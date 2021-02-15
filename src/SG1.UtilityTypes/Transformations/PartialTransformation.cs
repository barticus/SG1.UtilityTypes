using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes.Transformations
{
    internal sealed class PartialTransformationReader : BaseTransformationReader
    {
        public override string FullyQualifiedMetadataName => "SG1.UtilityTypes.PartialAttribute";

        public override string AttributeContent => @"using System;

namespace SG1.UtilityTypes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class PartialAttribute : Attribute
    {
        public PartialAttribute(Type sourceType, Type? nullableType = null, bool wrapReferenceTypes = false)
        {
        }
    }
}
";

        protected override ITransformation? ReadTransformationData(AttributeData partialAttributeData, Compilation compilation)
        {
            var sourceType = partialAttributeData.ConstructorArguments[0].Value as INamedTypeSymbol;
            INamedTypeSymbol nullableType;
            if (partialAttributeData.ConstructorArguments[1].IsNull)
            {
                nullableType = compilation.GetTypeByMetadataName("System.Nullable`1")!;
            }
            else if (partialAttributeData.ConstructorArguments[1].Value is INamedTypeSymbol namedTypeSymbol)
            {
                nullableType = namedTypeSymbol.ConstructedFrom;
            }
            else
            {
                throw new InvalidOperationException("Argument could not be read as an INamedTypeSymbol");
            }
            if (!nullableType.IsGenericType || nullableType.TypeArguments.Length != 1)
            {
                throw new InvalidOperationException($"{nullableType} needs to be a generic type with 1 argument");
                //     TODO: report diagnostic here
            }

            var wrapReferenceTypes = partialAttributeData.ConstructorArguments[2].Value as bool?;
            if (sourceType == null || nullableType == null || wrapReferenceTypes == null)
                return null;

            return new PartialTransformation(sourceType, nullableType, wrapReferenceTypes.Value);
        }
    }

    internal sealed class PartialTransformation : BaseTransformation
    {
        public INamedTypeSymbol NullableType { get; }
        public bool WrapReferenceTypes { get; }

        public PartialTransformation(INamedTypeSymbol sourceType, INamedTypeSymbol nullableType, bool wrapReferenceTypes) : base(sourceType)
        {
            NullableType = nullableType;
            WrapReferenceTypes = wrapReferenceTypes;
        }

        private bool ShouldWrapType(ITypeSymbol type, INamedTypeSymbol? nullableType, bool wrapReferenceTypes)
        {
            if (type.IsReferenceType && !wrapReferenceTypes)
            {
                return false;
            }

            if (nullableType == null)
            {
                return false;
            }

            return true;
        }

        public override string? GetPropertyType(IPropertySymbol property)
        {
            var wrapType = ShouldWrapType(property.Type, NullableType, WrapReferenceTypes);
            return wrapType ? NullableType.Construct(property.Type).ToString() : property.Type.WithNullableAnnotation(NullableAnnotation.Annotated).ToString();
        }
    }
}