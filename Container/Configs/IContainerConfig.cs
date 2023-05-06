using System;
using System.Collections.Generic;
using Mmvm.Mapper.Profile;

namespace Mmvm.Container.Configs
{
    public interface IContainerConfig<TContainer>
    {
        TContainer GetContainer(IMapperProfile mapperProfile, ICollection<Type> registrationTypes);
    }
}