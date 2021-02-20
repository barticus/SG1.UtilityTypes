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
        public OmitAttribute(Type sourceType, params string[] properties)
        {
        }
    }
}
";

        protected override ITransformation? ReadTransformationData(AttributeData attributeData, Compilation compilation)
        {
            var sourceType = GetConstructorArgument<INamedTypeSymbol>(attributeData, 0);
            var properties = GetConstructorArgumentValues<string>(attributeData, 1);
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