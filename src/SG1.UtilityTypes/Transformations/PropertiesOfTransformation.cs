using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes.Transformations
{
    internal sealed class PropertiesOfTransformationReader : ApplyTransformationReader
    {
        public override string FullyQualifiedMetadataName => "SG1.UtilityTypes.PropertiesOfAttribute";

        public override string AttributeContent => @"using System;

namespace SG1.UtilityTypes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class PropertiesOfAttribute : ApplyTransformAttribute
    {
        public PropertiesOfAttribute(Type sourceType): base(sourceType)
        {
        }
    }
}
";
    }
}