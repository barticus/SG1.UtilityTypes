using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes.Transformations
{
    internal abstract class BaseTransformation : ITransformation
    {
        public INamedTypeSymbol SourceType { get; }

        public BaseTransformation(INamedTypeSymbol sourceType)
        {
            SourceType = sourceType;
        }

        public virtual ITypeSymbol? GetPropertyType(IPropertySymbol property) => null;

        public virtual bool? ShouldIncludeProperty(IPropertySymbol property) => null;

        public virtual bool? ShouldIncludePropertySetter(IPropertySymbol property) => null;
    }
}