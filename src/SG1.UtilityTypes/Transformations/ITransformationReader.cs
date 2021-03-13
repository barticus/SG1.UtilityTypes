using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes.Transformations
{
    internal interface ITransformationReader
    {
        string FullyQualifiedMetadataName { get; }
        string AttributeContent { get; }
        ITransformation? ReadTransformationData(AttributeData attributeData, Compilation compilation);
    }
}