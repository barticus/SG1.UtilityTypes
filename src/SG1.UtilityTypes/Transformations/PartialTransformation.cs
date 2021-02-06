using System.Linq;
using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes.Transformations
{
    internal sealed class PartialTransformationReader : ITransformationReader
    {
        public string AttributeContent => @"using System;

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

        public ITransformation[] ReadTransformations(Compilation compilation, ITypeSymbol candidateTypeSymbol)
        {
            var attributeType = compilation.GetTypeByMetadataName("SG1.UtilityTypes.PartialAttribute");
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

        private static ITransformation? ReadTransformationData(AttributeData partialAttributeData)
        {
            var sourceType = partialAttributeData.ConstructorArguments[0].Value as INamedTypeSymbol;
            var wrappingType = partialAttributeData.ConstructorArguments[1].Value as string;
            var wrapReferenceTypes = partialAttributeData.ConstructorArguments[2].Value as bool?;
            if (sourceType == null || wrappingType == null || wrapReferenceTypes == null)
                return null;

            return new PartialTransformation(sourceType, wrappingType, wrapReferenceTypes.Value);
        }
    }

    internal sealed class PartialTransformation : BaseTransformation
    {
        public string WrappingType { get; }
        public bool WrapReferenceTypes { get; }

        public PartialTransformation(INamedTypeSymbol sourceType, string wrappingType, bool wrapReferenceTypes) : base(sourceType)
        {
            WrappingType = wrappingType;
            WrapReferenceTypes = wrapReferenceTypes;
        }

        private bool ShouldWrapType(ITypeSymbol type, string? wrappingType, bool wrapReferenceTypes)
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

        public override string? GetPropertyType(IPropertySymbol property)
        {
            var wrapType = ShouldWrapType(property.Type, WrappingType, WrapReferenceTypes);
            return wrapType ? $"{WrappingType}<{property.Type}>" : property.Type.ToString();
        }
    }
}