using System;
using System.Collections.Generic;
using System.Windows;
using Mmvm.View.Binding.Model;

namespace Mmvm.View.Binding
{
    public interface IBindingEngine
    {
        ICollection<SimpleVmBinding> GetSimpleVmBindings(ICollection<Type> types);

        ResourceDictionary GetVmBindingDictionary(ICollection<Type> types);
    }
}