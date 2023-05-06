using System;
using System.Collections.Generic;
using System.Reflection;
using Israiloff.Mmvm.Net.Core.Services.AssemblyLoader.Dtos;

namespace Israiloff.Mmvm.Net.Core.Services.AssemblyLoader
{
    public interface IAssemblyLoader
    {
        LoadResultDto Load(string moduleRegex);

        ICollection<Assembly> GetAllLoadedAssemblies();

        ICollection<Type> GetAllLoadedTypes();
    }
}