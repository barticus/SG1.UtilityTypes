using SG1.UtilityTypes.Configuration;
using Microsoft.CodeAnalysis.Text;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Globalization;
using SG1.UtilityTypes.Transformations;

namespace SG1.UtilityTypes
{
    internal sealed class ClassBuilder
    {
        public ClassBuilder(TransformInformation information, ConfigurationValues configurationValues) =>
            this.Text = ClassBuilder.Build(information, configurationValues);

        private static string GetAccessibilityString(Accessibility accessibility)
        {
            switch (accessibility)
            {
                case Accessibility.Private:
                case Accessibility.Protected:
                case Accessibility.Public:
                case Accessibility.Internal:
                    return accessibility.ToString().ToLower();
            }
            return "";
        }

        private static string PrintProperty(IPropertySymbol property, ITypeSymbol propertyType, bool shouldIncludeSetter)
        {
            // assign a default value when the property did not have a nullable annotation and the type has not been changed
            var needsDefaultValue = shouldIncludeSetter && property.NullableAnnotation != NullableAnnotation.Annotated
                && SymbolEqualityComparer.Default.Equals(propertyType, property.Type);

            return String.Join(" ", new[] {
                // needs more work to get comment
                property.GetDocumentationCommentXml(CultureInfo.InvariantCulture),
                GetAccessibilityString(property.DeclaredAccessibility),
                propertyType.ToString(),
                property.Name,
                "{",
                "get;",
                shouldIncludeSetter ? "set;" : "",
                "}",
                needsDefaultValue ? "= default!;" : "",
            }.Where(i => i.Any()));
        }

        private static SourceText Build(TransformInformation information, ConfigurationValues configurationValues)
        {
            using var writer = new StringWriter();
            using var indentWriter = new IndentedTextWriter(writer,
                configurationValues.IndentStyle == IndentStyle.Tab ? "\t" : new string(' ', (int)configurationValues.IndentSize));

            var usingStatements = new SortedSet<string>();

            if (!information.InputType.IsValueType)
            {
                usingStatements.Add("using System;");
            };

            foreach (var usingStatement in usingStatements)
            {
                indentWriter.WriteLine(usingStatement);
            }

            if (usingStatements.Count > 0)
            {
                indentWriter.WriteLine();
            }

            if (!information.InputType.ContainingNamespace.IsGlobalNamespace)
            {
                indentWriter.WriteLine($"namespace {information.InputType.ContainingNamespace.ToDisplayString()}");
                indentWriter.WriteLine("{");
                indentWriter.Indent++;
            }

            indentWriter.WriteLine($"public partial class {information.InputType.Name}");
            indentWriter.WriteLine("{");
            indentWriter.Indent++;

            var groupedTransformations = information.Transformations.GroupBy(t => t.SourceType);
            foreach (var transformationsGroup in groupedTransformations)
            {
                var transformations = transformationsGroup.ToArray();
                var properties = transformationsGroup.Key.GetMembers().OfType<IPropertySymbol>().ToArray();
                foreach (var property in properties)
                {
                    var shouldInclude = transformations.Select(t => t.ShouldIncludeProperty(property)).Where(t => t.HasValue).LastOrDefault();
                    if (shouldInclude.HasValue && !shouldInclude.Value)
                    {
                        continue;
                    }

                    var propertyType = transformations.Select(t => t.GetPropertyType(property)).Where(t => t != null).LastOrDefault() ?? property.Type;
                    var shouldIncludePropertySetter = transformations.Select(t => t.ShouldIncludePropertySetter(property)).Where(t => t.HasValue).LastOrDefault();
                    indentWriter.WriteLine(
                        PrintProperty(
                            property,
                            propertyType,
                            shouldIncludePropertySetter ?? property.SetMethod != null
                        )
                    );
                }
            }

            indentWriter.Indent--;
            indentWriter.WriteLine("};");

            if (!information.InputType.IsValueType)
            {
                indentWriter.Indent--;
            }

            if (!information.InputType.ContainingNamespace.IsGlobalNamespace)
            {
                indentWriter.Indent--;
                indentWriter.WriteLine("}");
            }

            return SourceText.From(writer.ToString(), Encoding.UTF8);
        }

        public SourceText Text { get; private set; }
    }
}