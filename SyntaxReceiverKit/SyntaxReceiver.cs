using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxReceiverKit
{
    public abstract class SyntaxReceiver : ISyntaxContextReceiver
    {

        protected abstract void OnClassNodeVisit(ClassDeclarationSyntax syntax, SemanticModel model);

        protected abstract void OnMethodNodeVisit(MethodDeclarationSyntax syntax, SemanticModel model);

        protected abstract void OnPropertyNodeVisit(PropertyDeclarationSyntax syntax, SemanticModel model);

        protected abstract void OnFieldNodeVisit(FieldDeclarationSyntax syntax, SemanticModel model);

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            switch (context.Node)
            {
                case ClassDeclarationSyntax syntax:
                    {
                        OnClassNodeVisit(syntax, context.SemanticModel);
                        break;
                    }
                case MethodDeclarationSyntax syntax:
                    {
                        OnMethodNodeVisit(syntax, context.SemanticModel);
                        break;
                    }
                case FieldDeclarationSyntax syntax:
                    {
                        OnFieldNodeVisit(syntax, context.SemanticModel);
                        break;
                    }
                case PropertyDeclarationSyntax syntax:
                    {
                        OnPropertyNodeVisit(syntax, context.SemanticModel);
                        break;
                    }
            }
        }
    }
}
