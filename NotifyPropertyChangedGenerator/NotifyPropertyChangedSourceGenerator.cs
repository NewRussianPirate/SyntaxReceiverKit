using Microsoft.CodeAnalysis;
using SyntaxReceiverKit;
using System.Runtime.CompilerServices;
using System.Text;

namespace NotifyPropertyChangedGenerator
{
    [Generator]
    public class NotifyPropertyChangedSourceGenerator : ISourceGenerator
    {
        static readonly string _attributeSrc = @"using System;
namespace AutoNotify
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AutoNotifyAttribute : Attribute {}
}";
        private TypesWithAttributesReceiver _receiver = new(TypesWithAttributesReceiver.Targets.Fields, new[] { "AutoNotify.AutoNotifyAttribute" } );

        //Change first latter of given string to Upper without alloc a new string
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe private void ToUpperFirst(ref string str)
        {
            fixed (char* pRes = str)
                pRes[0] = char.ToUpper(str[0]);
        }

        private string GetPropertyName(string fieldName)
        {
            if (char.IsLetter(fieldName[0]))
            {
                if (char.IsUpper(fieldName[0]))
                    return fieldName;
                else
                {
                    string res = fieldName;
                    ToUpperFirst(ref res);
                    return res;
                }
            }
            else
            {
                var substr = fieldName.Substring(1);
                ToUpperFirst(ref substr);
                return substr;
            }
        }

        private static string AccessibilityToString(Accessibility accessibility) => accessibility switch
        { 
            Accessibility.Private => "private",
            Accessibility.Public => "public",
            Accessibility.Internal => "internal",
            _ => ""
        };

        public void Execute(GeneratorExecutionContext context)
        {
            StringBuilder src = new("//Auto-generated\nusing System.ComponentModel;\n", 40);
            string propertyName;
            string accessibility = "public";
            var iNotifySymbol = context.Compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged");
            foreach(var i in _receiver.CollectedSymbols["AutoNotify.AutoNotifyAttribute"])
            {
                if (i.Value is not IFieldSymbol symbol)
                    return;
                propertyName = GetPropertyName(symbol.Name);
                if (symbol.ContainingType is INamedTypeSymbol classSymbol)
                    accessibility = AccessibilityToString(classSymbol.DeclaredAccessibility);
                src.Append($@"namespace {symbol.ContainingNamespace.ToDisplayString()}
{{
    {accessibility} partial class {symbol.ContainingType.Name} : {iNotifySymbol.ToDisplayString()}
    {{
            public {symbol.Type.ToDisplayString()} {propertyName}
            {{
                get => {symbol.Name};
                set
                {{
                    {symbol.Name} = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""{propertyName}""));
                }}
            }}
    }}
}}
");
            }
            context.AddSource("AutoNotifyGenerated.g.cs", src.ToString());
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization(n => n.AddSource("AutoNotifyAttribute.g.cs", _attributeSrc));
            context.RegisterForSyntaxNotifications(() => _receiver);
        }
    }
}
