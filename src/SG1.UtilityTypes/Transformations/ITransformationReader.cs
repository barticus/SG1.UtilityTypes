using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes.Transformations
{
    internal interface ITransformationReader
    {
        string FullyQualifiedMetadataName { get; }
        string AttributeContent { get; }
        ITransformation[] ReadTransformations(Compilation compilation, ITypeSymbol candidateTypeSymbol);
    }
}