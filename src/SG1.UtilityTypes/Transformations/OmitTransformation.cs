using System.Linq;
using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes.Transformations
{
    internal sealed class OmitTransformationReader : BaseTransformationReader
    {
        public override string FullyQualifiedMetadataName => "SG1.UtilityTypes.OmitAttribute";
        public override string AttributeContent => @"using System;

namespace SG1.UtilityTypes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class OmitAttribute : Attribute
    {
        public OmitAttribute(Type sourceType, string[] properties)
        {
        }
    }
}
";

        protected override ITransformation? ReadTransformationData(AttributeData attributeData)
        {
            var sourceType = attributeData.ConstructorArguments[0].Value as INamedTypeSymbol;
            var properties = attributeData.ConstructorArguments[1].Values.Select(v => v.Value).OfType<string>().ToArray<string>() as string[];
            if (sourceType == null || properties == null || !properties.Any())
                return null;

            return new OmitTransformation(sourceType, properties);
        }
    }

    internal sealed class OmitTransformation : BaseTransformation
    {
        private string[] Properties { get; }
        public OmitTransformation(INamedTypeSymbol sourceType, string[] properties) : base(sourceType)
        {
            Properties = properties;
        }

        public override bool? ShouldIncludeProperty(IPropertySymbol property)
        {
            return !Properties.Contains(property.Name);
        }
    }
}