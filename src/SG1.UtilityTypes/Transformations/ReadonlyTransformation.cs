using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes.Transformations
{
    internal sealed class ReadonlyTransformationReader : ApplyTransformationReader
    {
        public override string FullyQualifiedMetadataName => "SG1.UtilityTypes.ReadonlyAttribute";

        public override string AttributeContent => @"using System;

#nullable enable
namespace SG1.UtilityTypes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ReadonlyAttribute : ApplyTransformAttribute
    {
        public ReadonlyAttribute(Type sourceType): base(sourceType)
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

            applyTransform.IsReadonly = true;

            return applyTransform;
        }
    }
}