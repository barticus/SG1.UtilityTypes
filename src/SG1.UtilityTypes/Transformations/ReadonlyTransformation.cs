using System.Linq;
using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes.Transformations
{
    internal sealed class ReadonlyTransformationReader : BaseTransformationReader
    {
        public override string FullyQualifiedMetadataName => "SG1.UtilityTypes.ReadonlyAttribute";

        public override string AttributeContent => @"using System;

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

        protected override ITransformation? ReadTransformationData(AttributeData attributeData)
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