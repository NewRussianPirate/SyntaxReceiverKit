using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SyntaxReceiverKit
{
    /// <summary>
    /// Collect types which have given attributes.
    /// </summary>
    public class TypesWithAttributesReceiver : CombinedReceiver<KeyValuePair<AttributeData, ISymbol/*Type with an attribute*/>>
    {
        [Flags]
        public enum Targets : ushort
        {
            Classes = 1,
            Methods = 2,
            Properties = 4,
            Fields = 8
        }

        private Targets _targets;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckAttributes(ISymbol symbol)
        {
            foreach(var a in symbol.GetAttributes())
                AddIfKeyExists(a.AttributeClass.ToDisplayString(), new KeyValuePair<AttributeData, ISymbol>(a, symbol));
        }

        protected override void OnClassNodeVisit(ClassDeclarationSyntax syntax, SemanticModel model)
        {
            if ((_targets & Targets.Classes) == Targets.Classes && model.GetDeclaredSymbol(syntax) is INamedTypeSymbol symbol)
                CheckAttributes(symbol);
        }

        protected override void OnMethodNodeVisit(MethodDeclarationSyntax syntax, SemanticModel model)
        {
            if (_targets.HasFlag(Targets.Methods) && model.GetDeclaredSymbol(syntax) is IMethodSymbol symbol)
                CheckAttributes(symbol);
        }

        protected override void OnPropertyNodeVisit(PropertyDeclarationSyntax syntax, SemanticModel model)
        {
            if (_targets.HasFlag(Targets.Properties) && model.GetDeclaredSymbol(syntax) is IPropertySymbol symbol)
                CheckAttributes(symbol);
        }

        protected override void OnFieldNodeVisit(FieldDeclarationSyntax syntax, SemanticModel model)
        {
            if (!_targets.HasFlag(Targets.Fields))
                return;
            foreach(var fields in syntax.Declaration.Variables)
            {
                if (model.GetDeclaredSymbol(fields) is IFieldSymbol symbol)
                    CheckAttributes(symbol);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targets">Types which should be checked & collected.</param>
        /// <param name="attributes">An array with attributes names.</param>
        public TypesWithAttributesReceiver(Targets targets, string[] attributes) : base(attributes)
        {
            _targets = targets;
        }
    }
}
