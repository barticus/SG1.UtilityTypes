using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes.Transformations
{
    internal class ApplyTransformationReader : BaseTransformationReader
    {
        public override string FullyQualifiedMetadataName => "SG1.UtilityTypes.ApplyTransformAttribute";
        public override string AttributeContent => @"using System;

#nullable enable
namespace SG1.UtilityTypes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class ApplyTransformAttribute : Attribute
    {
        public bool IsReadonly { get; set; } = default!;
        public string[] IncludeProperties { get; set; } = default!;
        public string[] ExcludeProperties { get; set; } = default!;
        public ApplyTransformAttribute(Type sourceType)
        {
        }
    }
}
#nullable restore
";

        protected ApplyTransform? ReadApplyTransform(AttributeData attributeData, Compilation compilation)
        {
            var sourceType = GetConstructorArgument<INamedTypeSymbol>(attributeData, 0);
            var isReadonly = GetNamedArgument(attributeData, "IsReadonly") as bool?;
            var includeProperties = GetNamedArguments<string>(attributeData, "IncludeProperties");
            var excludeProperties = GetNamedArguments<string>(attributeData, "ExcludeProperties");
            if (sourceType == null)
                return null;

            return new ApplyTransform(sourceType)
            {
                IsReadonly = isReadonly,
                IncludeProperties = includeProperties,
                ExcludeProperties = excludeProperties
            };
        }

        public override ITransformation? ReadTransformationData(AttributeData attributeData, Compilation compilation)
        {
            return ReadApplyTransform(attributeData, compilation);
        }
    }

    internal class ApplyTransform : BaseTransformation
    {
        public bool? IsReadonly { get; set; }
        public string[]? IncludeProperties { get; set; }
        public string[]? ExcludeProperties { get; set; }

        public ApplyTransform(ApplyTransform applyTransform) : base(applyTransform.SourceType)
        {
            IsReadonly = applyTransform.IsReadonly;
            IncludeProperties = applyTransform.IncludeProperties;
            ExcludeProperties = applyTransform.ExcludeProperties;
        }

        public ApplyTransform(INamedTypeSymbol sourceType) : base(sourceType) { }

        public override bool? ShouldIncludePropertySetter(IPropertySymbol property) => IsReadonly.HasValue ? !IsReadonly : null;

        public override bool? ShouldIncludeProperty(IPropertySymbol property)
        {
            if (IncludeProperties != null)
            {
                return IncludeProperties.Contains(property.Name);
            }

            if (ExcludeProperties != null)
            {
                return !ExcludeProperties.Contains(property.Name);
            }

            return null;
        }
    }
}