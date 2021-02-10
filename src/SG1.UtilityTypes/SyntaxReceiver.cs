using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SG1.UtilityTypes
{
    class SyntaxReceiver : ISyntaxReceiver
    {
        public List<TypeDeclarationSyntax> Candidates { get; } = new List<TypeDeclarationSyntax>();

        public bool IsMatchingAttributeName(string attributeName)
        {
            return UtilityTypeGenerator.AttributeNames.Any(a =>
                    a == attributeName ||
                    a == $"{attributeName}Attribute" ||
                    a.EndsWith($".{attributeName}") ||
                    a.EndsWith($".{attributeName}Attribute")
                );
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is TypeDeclarationSyntax typeDeclarationSyntax)
            {
                foreach (var attributeList in typeDeclarationSyntax.AttributeLists)
                {
                    if (attributeList.Attributes.Any(a => IsMatchingAttributeName(a.Name.ToString())))
                    {
                        this.Candidates.Add(typeDeclarationSyntax);
                    }
                }
            }
        }
    }
}