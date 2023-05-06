using System;
using System.Collections.Generic;

namespace Mmvm.View.Binding
{
    public interface IResourceInitializer
    {
        void Initialize(ICollection<Type> types);
    }
}