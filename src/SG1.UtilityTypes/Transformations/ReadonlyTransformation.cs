using System.Linq;
using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes.Transformations
{
    internal sealed class ReadonlyTransformationReader : ITransformationReader
    {
        public string AttributeContent => @"using System;

namespace SG1.UtilityTypes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ReadonlyAttribute : Attribute
    {
        public ReadonlyAttribute(Type sourceType)
        {
        }
    }
}
";

        public ITransformation[] ReadTransformations(Compilation compilation, ITypeSymbol candidateTypeSymbol)
        {
            var attributeType = compilation.GetTypeByMetadataName("SG1.UtilityTypes.ReadonlyAttribute");
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
            if (sourceType == null)
                return null;

            return new ReadonlyTransformation(sourceType);
        }
    }

    internal sealed class ReadonlyTransformation : BaseTransformation
    {

        public ReadonlyTransformation(INamedTypeSymbol sourceType) : base(sourceType)
        {
        }

        public override bool? ShouldIncludePropertySetter(IPropertySymbol property) => false;
    }
}