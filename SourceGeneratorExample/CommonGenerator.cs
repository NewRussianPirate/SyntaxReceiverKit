﻿using Microsoft.CodeAnalysis;
using SyntaxReceiverKit;
using SyntaxReceiverKit.SymbolExtensions;
using System.Linq;
using System.Text;

namespace SourceGeneratorExample
{
    [Generator]
    internal class CommonGenerator : ISourceGenerator
    {
        private const string _attributesSrc = @"namespace SpecialAttributes
{
    [System.AttributeUsage(AttributeTargets.Class)]
    internal class SpecialClassAttribute : Attribute
    {
    }

    [System.AttributeUsage(AttributeTargets.Method)]
    internal class SpecialMethodAttribute : Attribute
    {
    }
}";

        private CommonSyntaxReceiver _receiver = new(classesCollectPredicate: n => n.AllInterfaces.Any(n=>n.ToDisplayString() == "MyNamespace.IMyInterface"), methodsCollectPredicate:
            n => n.HasAttribute("SpecialAttributes.SpecialMethodAttribute"));

        public void Execute(GeneratorExecutionContext context)
        {
            StringBuilder methodsSrc = new("//Autogenerated code\n");
            StringBuilder classesSrc = new("//Autogenerated code\n");
            foreach (var i in _receiver.Classes)
            {
                classesSrc.Append($@"namespace {i.ContainingNamespace.ToDisplayString()}
{{
    public partial class {i.Name}
    {{
        public string AutoGeneratedProperty => ""This is auto-generated property."";
    }}
}}"
);
            }

            for(int i = 0; i < _receiver.Methods.Count; i++)
            {
                methodsSrc.Append($@"namespace {_receiver.Methods[i].ContainingNamespace.ToDisplayString()}
{{
    public partial class {_receiver.Methods[i].ContainingSymbol.Name}
    {{
        public static partial void {_receiver.Methods[i].Name}()
        {{
            Console.WriteLine(""Hello from auto-generated method n {i}!"");
        }}
    }}
}}");
            }

            context.AddSource("AutoGeneratedMethods.g.cs", methodsSrc.ToString());
            context.AddSource("AutoGeneratedClasses.g.cs", classesSrc.ToString());
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization(n => n.AddSource("SpecialAttributes.g.cs", _attributesSrc));
            context.RegisterForSyntaxNotifications(() =>_receiver);
        }
    }
}