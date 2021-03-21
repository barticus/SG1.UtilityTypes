using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes.Transformations
{
    internal sealed class OmitTransformationReader : ApplyTransformationReader
    {
        public override string FullyQualifiedMetadataName => "SG1.UtilityTypes.OmitAttribute";
        public override string AttributeContent => @"using System;

#nullable enable
namespace SG1.UtilityTypes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class OmitAttribute : ApplyTransformAttribute
    {
        public OmitAttribute(Type sourceType, params string[] properties): base(sourceType)
        {
        }
    }
}
#nullable restore
";

        public override ITransformation? ReadTransformationData(AttributeData attributeData, Compilation compilation)
        {
            var applyTransform = this.ReadApplyTransform(attributeData, compilation);
            var properties = GetConstructorArgumentValues<string>(attributeData, 1);
            if (applyTransform == null || properties == null || !properties.Any())
                return null;

            applyTransform.ExcludeProperties = properties;

            return applyTransform;
        }
    }
}