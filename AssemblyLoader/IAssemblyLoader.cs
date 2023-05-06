using System;
using System.Collections.Generic;
using Mmvm.Assembly.Loader.Model;

namespace Mmvm.Assembly.Loader
{
    public interface IAssemblyLoader
    {
        LoadResultDto Load(string moduleRegex);

        ICollection<System.Reflection.Assembly> GetAllLoadedAssemblies();

        ICollection<Type> GetAllLoadedTypes();
    }
}