using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace SyntaxReceiverKit
{
    /// <summary>
    /// Collect any types with a specific criterias.
    /// </summary>
    public class CommonSyntaxReceiver : SyntaxReceiver
    {
        protected Predicate<INamedTypeSymbol> _classesCollectPred;
        protected Predicate<IMethodSymbol> _methodsCollectPred;
        protected Predicate<IFieldSymbol> _fieldsCollectPred;
        protected Predicate<IPropertySymbol> _propertiesCollectPred;

        public List<INamedTypeSymbol> Classes { get; } = new();
        public List<IMethodSymbol> Methods { get; } = new();
        public List<IPropertySymbol> Properties { get; } = new();
        public List<IFieldSymbol> Fields { get; } = new();

        /// <summary>
        /// Collect any types with a specified criterias.
        /// </summary>
        /// <param name="classesCollectPredicate">A criteria for collecting <see cref="INamedTypeSymbol"/>. Pass null to skip all <see cref="INamedTypeSymbol"/>.</param>
        /// <param name="methodsCollectPredicate">A criteria for collecting <see cref="IMethodSymbol"/>. Pass null to skip all <see cref="IMethodSymbol"/>.</param>
        /// <param name="fieldsCollectPredicate">A criteria for collecting <see cref="IFieldSymbol"/>. Pass null to skip all <see cref="IFieldSymbol"/>.</param>
        /// <param name="propertiesCollectPredicate">A criteria for collecting <see cref="IPropertySymbol"/>. Pass null to skip all <see cref="IPropertySymbol"/>.</param>
        public CommonSyntaxReceiver(Predicate<INamedTypeSymbol> classesCollectPredicate = null, Predicate<IMethodSymbol> methodsCollectPredicate = null,
            Predicate<IFieldSymbol> fieldsCollectPredicate = null, Predicate<IPropertySymbol> propertiesCollectPredicate = null)
        {
            if(classesCollectPredicate == null && methodsCollectPredicate == null && fieldsCollectPredicate == null && propertiesCollectPredicate == null)
                throw new ArgumentNullException("All predecates are null.");
            _classesCollectPred = classesCollectPredicate;
            _methodsCollectPred = methodsCollectPredicate;
            _fieldsCollectPred = fieldsCollectPredicate;
            _propertiesCollectPred = propertiesCollectPredicate;
        }

        /// <summary>
        /// Collect only classes with a specified criteria.
        /// </summary>
        /// <param name="classesCollectPredicate">A criteria for collecting <see cref="INamedTypeSymbol"/>.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CommonSyntaxReceiver(Predicate<INamedTypeSymbol> classesCollectPredicate)
        {
            if(classesCollectPredicate == null)
                throw new ArgumentNullException($"{nameof(classesCollectPredicate)} is null.");
            _classesCollectPred = classesCollectPredicate;
        }

        /// <summary>
        /// Collect only methods with a specified criteria.
        /// </summary>
        /// <param name="methodsCollectPredicate">A criteria for collecting <see cref="IMethodSymbol"/>.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CommonSyntaxReceiver(Predicate<IMethodSymbol> methodsCollectPredicate)
        {
            if (methodsCollectPredicate == null)
                throw new ArgumentNullException($"{nameof(methodsCollectPredicate)} is null.");
            _methodsCollectPred = methodsCollectPredicate;
        }

        /// <summary>
        /// Collect only fields with a specified criteria.
        /// </summary>
        /// <param name="fieldsCollectPredicate">A criteria for collecting <see cref="IFieldSymbol"/>.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CommonSyntaxReceiver(Predicate<IFieldSymbol> fieldsCollectPredicate)
        {
            if (fieldsCollectPredicate == null)
                throw new ArgumentNullException($"{nameof(fieldsCollectPredicate)} is null.");
            _fieldsCollectPred = fieldsCollectPredicate;
        }

        /// <summary>
        /// Collect only properties with a specified criteria.
        /// </summary>
        /// <param name="propertiesCollectPredicate">A criteria for collecting <see cref="IPropertySymbol"/>.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CommonSyntaxReceiver(Predicate<IPropertySymbol> propertiesCollectPredicate)
        {
            if (propertiesCollectPredicate == null)
                throw new ArgumentNullException($"{nameof(propertiesCollectPredicate)} is null.");
            _propertiesCollectPred = propertiesCollectPredicate;
        }

        protected override void OnClassNodeVisit(ClassDeclarationSyntax syntax, SemanticModel model)
        {
            if (_classesCollectPred != null && model.GetDeclaredSymbol(syntax) is INamedTypeSymbol symbol && _classesCollectPred(symbol))
                Classes.Add(symbol);
        }

        protected override void OnMethodNodeVisit(MethodDeclarationSyntax syntax, SemanticModel model)
        {
            if (_methodsCollectPred != null && model.GetDeclaredSymbol(syntax) is IMethodSymbol symbol && _methodsCollectPred(symbol))
                Methods.Add(symbol);
        }

        protected override void OnPropertyNodeVisit(PropertyDeclarationSyntax syntax, SemanticModel model)
        {
            if (_fieldsCollectPred != null && model.GetDeclaredSymbol(syntax) is IPropertySymbol symbol && _propertiesCollectPred(symbol))
                Properties.Add(symbol);
        }

        protected override void OnFieldNodeVisit(FieldDeclarationSyntax syntax, SemanticModel model)
        {
            if (_fieldsCollectPred == null)
                return;
            foreach (var fields in syntax.Declaration.Variables)
            {
                if (model.GetDeclaredSymbol(fields) is IFieldSymbol symbol && _fieldsCollectPred(symbol))
                    Fields.Add(symbol);
            }
        }
    }
}