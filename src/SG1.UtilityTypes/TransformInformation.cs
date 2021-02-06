using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace SG1.UtilityTypes
{
    internal sealed class TransformInformation
    {
        public ITypeSymbol InputType { get; }

        public IList<ITransformation> Transformations { get; } = new List<ITransformation>();

        public TransformInformation(ITypeSymbol inputType)
        {
            InputType = inputType;
        }
    }

    internal interface ITransformation { }

    internal sealed class PartialTransformation : ITransformation
    {
        public IPropertySymbol[] Fields { get; }
        public string WrappingType { get; }
        public bool WrapReferenceTypes { get; }

        public PartialTransformation(INamedTypeSymbol sourceType, string wrappingType, bool wrapReferenceTypes)
        {
            Fields = sourceType.GetMembers().OfType<IPropertySymbol>().ToArray();
            WrappingType = wrappingType;
            WrapReferenceTypes = wrapReferenceTypes;
        }
    }
}