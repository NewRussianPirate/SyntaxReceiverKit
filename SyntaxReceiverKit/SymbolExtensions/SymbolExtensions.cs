using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyntaxReceiverKit.SymbolExtensions
{
    public static class SymbolExtensions
    {
        public static bool ContainNamespace(this ISymbol symbol, string namespacePart)
        {
            if(symbol.ContainingNamespace == null)
                return false;
            var namespaceParts = symbol.ContainingNamespace.ToDisplayString().Split('.');
            return namespaceParts.Any(n => n == namespacePart);
        }

        public static bool HasAttribute(this ISymbol symbol, string attributeName) => symbol.GetAttributes().Any(n => n.AttributeClass.ToDisplayString() == attributeName);
    }
}
