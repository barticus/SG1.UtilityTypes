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

        private static bool ShouldWrapType(ITypeSymbol type, string? wrappingType, bool wrapReferenceTypes)
        {
            if (type.IsReferenceType && !wrapReferenceTypes)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(wrappingType))
            {
                return false;
            }

            return true;
        }


        private static string GetTypeName(ITypeSymbol type, string? wrappingType, bool wrapReferenceTypes)
        {
            if (type.IsReferenceType && !wrapReferenceTypes)
            {
                return type.ToString();
            }

            if (string.IsNullOrWhiteSpace(wrappingType))
            {
                return type.ToString();
            }

            return $"{wrappingType}<{type}>";
        }

        private static string PrintProperty(IPropertySymbol property, string? wrappingType, bool wrapReferenceTypes)
        {
            var wrapType = ShouldWrapType(property.Type, wrappingType, wrapReferenceTypes);
            return String.Join(" ", new[] {
                // needs more work to get comment
                property.GetDocumentationCommentXml(CultureInfo.InvariantCulture),
                GetAccessibilityString(property.DeclaredAccessibility),
                wrapType ? $"{wrappingType}<{property.Type}>" : property.Type.ToString(),
                property.Name,
                "{",
                property.GetMethod != null ? "get;" : "",
                property.SetMethod != null ? "set;" : "",
                "}",
                property.NullableAnnotation != NullableAnnotation.Annotated  && !wrapType ? " = default!;" : "",
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

            foreach (var partialTransformation in information.Transformations.OfType<PartialTransformation>())
            {
                if (partialTransformation == null)
                    continue;

                foreach (var field in partialTransformation.Fields)
                {
                    indentWriter.WriteLine(
                        PrintProperty(
                            field,
                            partialTransformation.WrappingType,
                            partialTransformation.WrapReferenceTypes
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