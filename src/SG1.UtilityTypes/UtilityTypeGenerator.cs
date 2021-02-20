using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using SG1.UtilityTypes.Configuration;
using SG1.UtilityTypes.Transformations;

namespace SG1.UtilityTypes
{
    [Generator]
    public class UtilityTypeGenerator : ISourceGenerator
    {
        private static ITransformationReader[] TransformationReaders = new ITransformationReader[] {
            new PartialTransformationReader(),
            new PickTransformationReader(),
            new ReadonlyTransformationReader(),
            new OmitTransformationReader(),
            new PropertiesOfTransformationReader(),
        };

        public static string[] AttributeNames => TransformationReaders.Select(tr => tr.FullyQualifiedMetadataName).ToArray();

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
            return compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(text, Encoding.UTF8), options));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            foreach (var tr in TransformationReaders)
            {
                context.AddSource(tr.GetType().Name, SourceText.From(tr.AttributeContent, Encoding.UTF8));
            }

            // retreive the populated receiver 
            if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
                return;

            Compilation compilation = context.Compilation;
            foreach (var tr in TransformationReaders)
            {
                compilation = AddToSyntaxTree(compilation, tr.AttributeContent);
            }

            foreach (var candidateTypeNode in receiver.Candidates)
            {
                var model = compilation.GetSemanticModel(candidateTypeNode.SyntaxTree);
                var candidateTypeSymbol = model.GetDeclaredSymbol(candidateTypeNode) as ITypeSymbol;
                if (candidateTypeSymbol == null)
                    continue;

                var transformations = new List<ITransformation>();
                foreach (var tr in TransformationReaders)
                {
                    transformations.AddRange(
                        tr.ReadTransformations(compilation, candidateTypeSymbol)
                    );
                }

                if (transformations.Any())
                {
                    var configurationValues = new ConfigurationValues(context, candidateTypeNode.SyntaxTree);
                    var information = new TransformInformation(candidateTypeSymbol, transformations);
                    var content = new ClassBuilder(information, configurationValues).Text;
                    var name = $"{information.InputType.Name}.cs";
                    context.AddSource(name, content);
                }
            }
        }
    }
}