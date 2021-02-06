using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using SG1.UtilityTypes.Configuration;

namespace SG1.UtilityTypes
{
    [Generator]
    public class UtilityTypeGenerator : ISourceGenerator
    {
        private const string partialAttributeText = @"using System;

namespace SG1.UtilityTypes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class PartialAttribute : Attribute
    {
        public PartialAttribute(Type sourceType, string wrappingType = ""Nullable"", bool wrapReferenceTypes = false)
        {
        }
    }
}
";

        public void Initialize(GeneratorInitializationContext context)
        {
            // Register a syntax receiver that will be created for each generation pass
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        private Compilation AddToSyntaxTree(Compilation compilation, string text)
        {
            if (!(compilation is CSharpCompilation cSharpCompilation))
            {
                throw new InvalidOperationException("compilation is not CSharpCompilation");
            }
            if (!(cSharpCompilation.SyntaxTrees[0].Options is CSharpParseOptions options))
            {
                throw new InvalidOperationException("Options is not CSharpParseOptions");
            }
            return compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(partialAttributeText, Encoding.UTF8), options));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource("PartialAttribute", SourceText.From(partialAttributeText, Encoding.UTF8));

            // retreive the populated receiver 
            if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
                return;

            var compilation = AddToSyntaxTree(context.Compilation, partialAttributeText);

            var partialAttributeType = compilation.GetTypeByMetadataName("SG1.UtilityTypes.PartialAttribute");
            foreach (var candidateTypeNode in receiver.Candidates)
            {
                var model = compilation.GetSemanticModel(candidateTypeNode.SyntaxTree);
                var candidateTypeSymbol = model.GetDeclaredSymbol(candidateTypeNode) as ITypeSymbol;
                if (candidateTypeSymbol == null)
                    continue;
                foreach (var partialAttribute in candidateTypeSymbol.GetAttributes()
                    .Where(
                        _ => _.AttributeClass!.Equals(
                            partialAttributeType, SymbolEqualityComparer.Default
                        )
                    )
                )
                {
                    var sourceType = partialAttribute.ConstructorArguments[0].Value as INamedTypeSymbol;
                    var wrappingType = partialAttribute.ConstructorArguments[1].Value as string;
                    var wrapReferenceTypes = partialAttribute.ConstructorArguments[2].Value as bool?;
                    if (sourceType == null || wrappingType == null || wrapReferenceTypes == null)
                        continue;


                    var configurationValues = new ConfigurationValues(context, candidateTypeNode.SyntaxTree);
                    var information = new TransformInformation(candidateTypeSymbol);
                    information.Transformations.Add(new PartialTransformation(sourceType, wrappingType, wrapReferenceTypes.Value));
                    var content = new ClassBuilder(information, configurationValues).Text;
                    var name = $"{information.InputType.Name}.cs";
                    context.AddSource(name, content);
                }
            }
        }
    }

    class SyntaxReceiver : ISyntaxReceiver
    {
        public List<TypeDeclarationSyntax> Candidates { get; } = new List<TypeDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is TypeDeclarationSyntax typeDeclarationSyntax)
            {
                foreach (var attributeList in typeDeclarationSyntax.AttributeLists)
                {
                    if (attributeList.Attributes.Any())
                    {
                        this.Candidates.Add(typeDeclarationSyntax);
                    }
                }
            }
        }
    }
}