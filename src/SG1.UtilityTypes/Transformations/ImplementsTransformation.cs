using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes.Transformations
{
    internal sealed class ImplementsTransformationReader : ApplyTransformationReader
    {
        public override string FullyQualifiedMetadataName => "SG1.UtilityTypes.ImplementsAttribute";

        public override string AttributeContent => @"using System;

#nullable enable
namespace SG1.UtilityTypes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class ImplementsAttribute : ApplyTransformAttribute
    {
        public ImplementsAttribute(Type sourceType): base(sourceType)
        {
        }
    }
}
#nullable restore
";

        public override ITransformation? ReadTransformationData(AttributeData attributeData, Compilation compilation)
        {
            var applyTransform = this.ReadApplyTransform(attributeData, compilation);
            if (applyTransform == null)
                return null;

            applyTransform.IsReadonly = applyTransform.IsReadonly ?? false;

            return applyTransform;
        }
    }
}