using System.Linq;
using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes.Transformations
{
    internal sealed class PickTransformationReader : ITransformationReader
    {
        public string AttributeContent => @"using System;

namespace SG1.UtilityTypes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class PickAttribute : Attribute
    {
        public PickAttribute(Type sourceType, string[] properties)
        {
        }
    }
}
";

        public ITransformation[] ReadTransformations(Compilation compilation, ITypeSymbol candidateTypeSymbol)
        {
            var attributeType = compilation.GetTypeByMetadataName("SG1.UtilityTypes.PickAttribute");
            return candidateTypeSymbol
                .GetAttributes()
                .Where(
                    ad => ad.AttributeClass!.Equals(
                        attributeType, SymbolEqualityComparer.Default
                    )
                )
                .Select(ReadTransformationData)
                .Where(t => t != null)
                .Select(t => t!)
                .ToArray();
        }

        private static ITransformation? ReadTransformationData(AttributeData attributeData)
        {
            var sourceType = attributeData.ConstructorArguments[0].Value as INamedTypeSymbol;
            var properties = attributeData.ConstructorArguments[1].Values.Select(v => v.Value).OfType<string>().ToArray<string>() as string[];
            if (sourceType == null || properties == null || !properties.Any())
                return null;

            return new PickTransformation(sourceType, properties);
        }
    }

    internal sealed class PickTransformation : BaseTransformation
    {
        private string[] Properties { get; }
        public PickTransformation(INamedTypeSymbol sourceType, string[] properties) : base(sourceType)
        {
            Properties = properties;
        }

        public override bool? ShouldIncludeProperty(IPropertySymbol property)
        {
            return Properties.Contains(property.Name);
        }
    }
}