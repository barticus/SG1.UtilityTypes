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
        public PartialAttribute(Type sourceType, Type? nullableType = null, bool wrapAlreadyNullTypes = false)
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
        public bool WrapAlreadyNullTypes { get; }

        public PartialTransformation(INamedTypeSymbol sourceType, INamedTypeSymbol nullableType, bool wrapAlreadyNullTypes) : base(sourceType)
        {
            NullableType = nullableType;
            WrapAlreadyNullTypes = wrapAlreadyNullTypes;
        }

        private bool ShouldWrapType(ITypeSymbol type)
        {
            if (WrapAlreadyNullTypes)
            {
                return true;
            }

            return type.NullableAnnotation != NullableAnnotation.Annotated;
        }

        public override ITypeSymbol? GetPropertyType(IPropertySymbol property)
        {
            return ShouldWrapType(property.Type) ?
                NullableType.Construct(property.Type) :
                property.Type;
        }
    }
}