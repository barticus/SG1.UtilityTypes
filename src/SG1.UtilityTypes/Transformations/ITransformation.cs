using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes.Transformations
{
    internal interface ITransformation
    {
        INamedTypeSymbol SourceType { get; }
        bool? ShouldIncludeProperty(IPropertySymbol property);
        string? GetPropertyType(IPropertySymbol property);
        bool? ShouldIncludePropertySetter(IPropertySymbol property);
    }
}