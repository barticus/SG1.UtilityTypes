using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes.Transformations
{
    public class BaseTransformation : ITransformation
    {
        public INamedTypeSymbol SourceType { get; }

        public BaseTransformation(INamedTypeSymbol sourceType)
        {
            SourceType = sourceType;
        }

        public virtual string? GetPropertyType(IPropertySymbol property) => null;

        public virtual bool? ShouldIncludeProperty(IPropertySymbol property) => null;

        public virtual bool? ShouldIncludePropertySetter(IPropertySymbol property) => null;
    }
}