using System.Linq;
using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes.Transformations
{
    internal sealed class PropertiesOfTransformationReader : BaseTransformationReader
    {
        public override string FullyQualifiedMetadataName => "SG1.UtilityTypes.PropertiesOfAttribute";

        public override string AttributeContent => @"using System;

namespace SG1.UtilityTypes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class PropertiesOfAttribute : Attribute
    {
        public PropertiesOfAttribute(Type sourceType)
        {
        }
    }
}
";

        protected override ITransformation? ReadTransformationData(AttributeData attributeData, Compilation compilation)
        {
            var sourceType = attributeData.ConstructorArguments[0].Value as INamedTypeSymbol;
            if (sourceType == null)
                return null;

            return new PropertiesOfTransformation(sourceType);
        }
    }

    internal sealed class PropertiesOfTransformation : BaseTransformation
    {
        public PropertiesOfTransformation(INamedTypeSymbol sourceType) : base(sourceType) { }
    }
}