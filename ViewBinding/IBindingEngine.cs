using System;
using System.Collections.Generic;
using System.Windows;
using Israiloff.Mmvm.Net.Mvvm.View.Services.BindingEngine.Dtos;

namespace Israiloff.Mmvm.Net.Mvvm.View.Services.BindingEngine
{
    public interface IBindingEngine
    {
        ICollection<SimpleVmBindingDto> GetSimpleVmBindings(ICollection<Type> types);

        ResourceDictionary GetVmBindingDictionary(ICollection<Type> types);
    }
}