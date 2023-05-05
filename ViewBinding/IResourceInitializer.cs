using System;
using System.Collections.Generic;

namespace Israiloff.Mmvm.Net.Mvvm.View.Services.ResourceInitializer
{
    public interface IResourceInitializer
    {
        void Initialize(ICollection<Type> types);
    }
}