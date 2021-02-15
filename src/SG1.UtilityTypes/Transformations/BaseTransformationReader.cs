using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes.Transformations
{
    internal abstract class BaseTransformationReader : ITransformationReader
    {
        public abstract string FullyQualifiedMetadataName { get; }
        public abstract string AttributeContent { get; }

        public List<Diagnostic> Diagnostics { get; } = new List<Diagnostic>();

        public ITransformation[] ReadTransformations(Compilation compilation, ITypeSymbol candidateTypeSymbol)
        {
            var attributeType = compilation.GetTypeByMetadataName(FullyQualifiedMetadataName);
            return candidateTypeSymbol
                .GetAttributes()
                .Where(
                    ad => ad.AttributeClass!.Equals(
                        attributeType, SymbolEqualityComparer.Default
                    )
                )
                .Select(ad => ReadTransformationData(ad, compilation))
                .Where(t => t != null)
                .Select(t => t!)
                .ToArray();
        }

        protected abstract ITransformation? ReadTransformationData(AttributeData attributeData, Compilation compilation);
    }
}