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

        protected object? GetConstructorArgument(AttributeData attributeData, int index)
        {
            if (attributeData.ConstructorArguments.Length > index)
            {
                return attributeData.ConstructorArguments[index].Value;
            }
            return null;
        }

        protected T? GetConstructorArgument<T>(AttributeData attributeData, int index) where T : class =>
            GetConstructorArgument(attributeData, index) as T;


        protected object[] GetConstructorArgumentValues(AttributeData attributeData, int index)
        {
            if (attributeData.ConstructorArguments.Length > index)
            {
                return attributeData.ConstructorArguments[index].Values
                    .Where(v => v.Value != null)
                    .Select(v => v.Value!)
                    .ToArray();
            }
            return new object[0];
        }

        protected T[] GetConstructorArgumentValues<T>(AttributeData attributeData, int index) where T : class =>
            GetConstructorArgumentValues(attributeData, index).OfType<T>().ToArray();

        protected object? GetNamedArgument(AttributeData attributeData, string key)
        {
            if (attributeData.NamedArguments.Any(a => a.Key == key))
            {
                return attributeData.NamedArguments.First(a => a.Key == key).Value.Value;
            }
            return null;
        }

        protected T? GetNamedArgument<T>(AttributeData attributeData, string key) where T : class =>
            GetNamedArgument(attributeData, key) as T;

        protected abstract ITransformation? ReadTransformationData(AttributeData attributeData, Compilation compilation);
    }
}