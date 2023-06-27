# SyntaxReceiverKit
A bunch of syntax receivers for C# source generators.

# Find types by specific conditions
```
using SyntaxReceiverKit;

namespace GeneratorExample
{
	[Generator]
	internal class CommonGenerator : ISourceGenerator
	{
		 //Collect classes that implements "MyNamespace.IMyInterface" and methods with "Cool" attribute:
		 private CommonSyntaxReceiver _receiver = new(classesCollectPredicate: n => n.AllInterfaces.Any(n => n.ToDisplayString() == "MyNamespace.IMyInterface"), methodsCollectPredicate:
			  n => n.HasAttribute("NamespaceName.CoolAttribute"));

		 public void Execute(GeneratorExecutionContext context)
		 {
			  foreach (var i in _receiver.Classes)
			  {
					//Do smth with classed...
			  }
	  
			  foreach(var i in _receiver.Methods)
			  {
					//Do smth with methods...
			  }
		 }

		 public void Initialize(GeneratorInitializationContext context)
		 {
			  context.RegisterForSyntaxNotifications(() =>_receiver);
		 }
	}
}
```

# Find all classes that are implements given interfaces
```
[Generator]
internal class InterfaceImplementGenerator : ISourceGenerator
{
    private readonly string[] _ifaceNames = new[] { "SomeNamespace.ISomeInterface", "SomeNamespace.ISomeAnotherInterface" };
    private InterfacesImplementReceiver _receiver;

    public void Execute(GeneratorExecutionContext context)
    {
        foreach (var i in _receiver.CollectedSymbols[_ifaceNames[0]])
        {
            //Do stuff with collected class info here
        }

        foreach (var i in _receiver.CollectedSymbols[_ifaceNames[1]])
        {
            //Do stuff with collected class info here
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        _receiver = new(_ifaceNames);
        context.RegisterForSyntaxNotifications(() => _receiver);
    }
}
```

# Find types with given attributes
```
[Generator]
internal class AttributeStuffGenerator : ISourceGenerator
{
    private TypesWithAttributesReceiver _receiver = new(TypesWithAttributesReceiver.Targets.Properties | TypesWithAttributesReceiver.TargetsMethods, new[] { "SomeNamespace.CoolAttribute", "SomeNamespace.NotCoolAttribute" } );

    public void Execute(GeneratorExecutionContext context)
    {
        foreach (var i in _receiver.CollectedSymbols["SomeNamespace.CoolAttribute"])
        {
            if(i.Value is IPropertySymbol propertySymbol)
            {
                //Do stuff...
            }
            else if (i.Value is IMethodSymbol methodSymbol)
            {
                //Do stuff...
            }
        }

        foreach (var i in _receiver.CollectedSymbols["SomeNamespace.NotCoolAttribute"])
        {
            if(i.Value is IPropertySymbol propertySymbol)
            {
                //Do stuff...
            }
            else if (i.Value is IMethodSymbol methodSymbol)
            {
                //Do stuff...
            }
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() =>_receiver);
    }
}
```