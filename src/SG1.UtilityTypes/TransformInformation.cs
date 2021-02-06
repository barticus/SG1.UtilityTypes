using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using SG1.UtilityTypes.Transformations;

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

        public TransformInformation(ITypeSymbol inputType, IList<ITransformation> transformations) : this(inputType)
        {
            Transformations = transformations;
        }
    }


}