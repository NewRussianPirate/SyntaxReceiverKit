using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SyntaxReceiverKit
{
    /// <summary>
    /// Collect all classes which implements interfaces with a specific names.
    /// </summary>
    public class InterfacesImplementReceiver : CombinedReceiverBase<INamedTypeSymbol, INamedTypeSymbol>
    {
        //Compare INamedTypeSymbol interfaces by full name;
        private class NamedSymbolStringComparer : IEqualityComparer<INamedTypeSymbol>
        {
            public bool Equals(INamedTypeSymbol x, INamedTypeSymbol y) => x.ToDisplayString() == y.ToDisplayString();

            public int GetHashCode(INamedTypeSymbol obj) => obj.GetHashCode();
        }

        private string[] _interfaces;

        public InterfacesImplementReceiver(string[] interfaces)
        {
            if (interfaces == null) throw new ArgumentNullException(nameof(interfaces));
            if (interfaces.Length == 0) throw new ArgumentException($"{nameof(interfaces)} is empty.");
            CollectedSymbols = new(new NamedSymbolStringComparer());
            _interfaces = interfaces;
        }

        protected override void OnClassNodeVisit(ClassDeclarationSyntax syntax, SemanticModel model)
        {
            if(model.GetDeclaredSymbol(syntax) is INamedTypeSymbol symbol)
            {
                foreach(var i in symbol.Interfaces) 
                {
                    if(_interfaces.Any(n=> n == i.ToDisplayString()))
                    {
                        try
                        {
                            CollectedSymbols[i].Add(symbol);
                        }
                        catch(KeyNotFoundException)
                        {
                            var list = new List<INamedTypeSymbol>() { symbol };
                            CollectedSymbols.Add(i, list);
                        }
                    }
                }
            }
        }

        protected override void OnFieldNodeVisit(FieldDeclarationSyntax syntax, SemanticModel model) { }

        protected override void OnMethodNodeVisit(MethodDeclarationSyntax syntax, SemanticModel model) { }

        protected override void OnPropertyNodeVisit(PropertyDeclarationSyntax syntax, SemanticModel model) { }
    }
}
