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
        public bool WrapAlreadyNullTypes { get; set; }
        public Type? NullableType { get; set; }

        public PartialAttribute(Type sourceType)
        {
        }
    }
}
";

        protected override ITransformation? ReadTransformationData(AttributeData attributeData, Compilation compilation)
        {
            var sourceType = GetConstructorArgument<INamedTypeSymbol>(attributeData, 0);

            var specifiedNullableType = GetNamedArgument<INamedTypeSymbol>(attributeData, "NullableType");
            var nullableType = specifiedNullableType != null ?
                specifiedNullableType.ConstructedFrom :
                compilation.GetTypeByMetadataName("System.Nullable`1")!;
            if (!nullableType.IsGenericType || nullableType.TypeArguments.Length != 1)
            {
                throw new InvalidOperationException($"{nullableType} needs to be a generic type with 1 argument");
                //     TODO: report diagnostic here
            }

            var wrapAlreadyNullTypes = GetNamedArgument(attributeData, "WrapAlreadyNullTypes") as bool? ?? false;
            if (sourceType == null || nullableType == null)
                return null;

            return new PartialTransformation(sourceType, nullableType, wrapAlreadyNullTypes);
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