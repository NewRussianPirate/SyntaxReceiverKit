using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyntaxReceiverKit.SymbolExtensions
{
    public static class NamedTypeSymbolExtension
    {
        public static bool IsDerivedFrom(this INamedTypeSymbol symbol, string typeName)
        {
            var baseType = symbol.BaseType;
            if (baseType == null)
                return false;
            if (baseType.ToDisplayString() == typeName)
                return true;
            return IsDerivedFrom(baseType, typeName);
        }

        public static bool IsImplementInterface(this INamedTypeSymbol symbol, string typeName) => symbol.Interfaces.Any(n=> n.ToDisplayString() == typeName);
    }
}
