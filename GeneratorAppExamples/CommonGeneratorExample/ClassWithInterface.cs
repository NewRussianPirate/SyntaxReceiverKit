using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeNamespace
{
    //InterfaceImplementGenerator will create a property for all classes that are implements this interface.
    interface ISomeInterface
    {

    }

    //InterfaceImplementGenerator will create a property for all classes that are implements this interface.
    interface ISomeAnotherInterface
    {

    }

    internal partial class ClassWithInterface : ISomeInterface
    {
    }

    internal partial class SecondClassWithInterface : ISomeInterface
    {
    }

    internal partial class ClassWithAnotherInterface : ISomeAnotherInterface
    {
    }

    internal partial class ClassWithBothIfaces : ISomeAnotherInterface, ISomeInterface
    {
    }
}
