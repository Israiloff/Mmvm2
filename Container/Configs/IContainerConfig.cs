using System;
using System.Collections.Generic;
using Israiloff.Mmvm.Net.Mvvm.Core.Model;

namespace Israiloff.Mmvm.Net.Container.Configs
{
    public interface IContainerConfig<TContainer>
    {
        TContainer GetContainer(IMapperProfile mapperProfile, ICollection<Type> registrationTypes);
    }
}