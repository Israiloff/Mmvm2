using System;
using System.Collections.Generic;
using Mmvm.Assembly.Loader.Model;

namespace Mmvm.Assembly.Loader
{
    public interface IAssemblyLoader
    {
        LoadResult AssemblyLoadResult { get; }

        ICollection<Type> LoadedTypes { get; }

        LoadResult Load(string moduleRegex);

        ICollection<Type> GetAllLoadedTypes();
    }
}