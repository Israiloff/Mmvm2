using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Autofac.Util;
using Israiloff.Cashbox.Component.Logger;
using Israiloff.Mmvm.Net.Container.Attributes;
using Israiloff.Mmvm.Net.Core.Services.AssemblyLoader;
using Israiloff.Mmvm.Net.Core.Services.AssemblyLoader.Dtos;

namespace Israiloff.Mmvm.Net.Core.Impl.Services.AssemblyLoader
{
    [Service(Name = nameof(CommonAssemblyLoader))]
    public class CommonAssemblyLoader : IAssemblyLoader
    {
        #region Constructors

        public CommonAssemblyLoader(ILogger logger)
        {
            Logger = logger;
        }

        #endregion

        #region Private properties

        private ILogger Logger { get; }

        private static string ExclusionsRegex =>
            "^((System)|(Microsoft)|(JetBrains)|(Newtonsoft)|(NLog)|(Autofac)|(EntityFramework))(\\.*.*)$";

        #endregion

        #region IAssemblyLoader impl

        public LoadResultDto Load(string moduleRegex)
        {
            Logger.Info("Load method started");

            var loadedAssemblies = GetLoadedAssemblies();
            var loadedPaths = ExtractAssemblyPath(loadedAssemblies);
            var referencedPaths = GetAllLocalDllPaths();
            var loadingAssemblies =
                FilterLoadingAssemblies(referencedPaths, loadedPaths, moduleRegex);

            if (loadingAssemblies.Count > 0)
            {
                Logger.Debug("Not loaded assemblies count : {0}. Assemblies will be load", loadingAssemblies.Count);
                LoadAssemblies(loadingAssemblies).ForEach(loadedAssemblies.Add);
                return MapAssembliesToResultDto(loadedAssemblies, moduleRegex);
            }

            Logger.Warn("All assemblies already loaded. Operation terminated");
            return MapAssembliesToResultDto(loadedAssemblies, moduleRegex);
        }

        public ICollection<Assembly> GetAllLoadedAssemblies()
        {
            Logger.Info("GetAllLoadedAssemblies started");
            var result = GetLoadedAssemblies();
            Logger.Debug("Loaded assemblies\' count : {0}", result.Count);
            return result;
        }

        public ICollection<Type> GetAllLoadedTypes()
        {
            Logger.Info("GetAllLoadedTypes started");
            var result = GetTypes(GetLoadedAssemblies());
            Logger.Debug("Loaded types\' count : {0}", result.Count);
            return result;
        }

        #endregion

        #region Private methods

        private LoadResultDto MapAssembliesToResultDto(IEnumerable<Assembly> assemblies,
            string moduleRegex)
        {
            Logger.Debug("MapAssembliesToResult started");

            var filteredAssemblies = assemblies
                .Where(assembly => MatchRegex(moduleRegex, assembly.FullName))
                .ToList();

            var result = new LoadResultDto(GetInjectableTypes(filteredAssemblies),
                GetAssembliesPaths(filteredAssemblies));

            Logger.Debug("Assemblies successfully mapped to result dto");

            return result;
        }

        private ICollection<string> GetAssembliesPaths(IEnumerable<Assembly> assemblies)
        {
            Logger.Debug("GetAssembliesPaths started");
            var result = assemblies.Select(assembly => assembly.Location).ToList();
            Logger.Debug("Assemblies\' paths successfully got. Count : {0}", result.Count);
            return result;
        }

        private ICollection<Type> GetTypes(IEnumerable<Assembly> assemblies)
        {
            Logger.Debug("GetTypes started");

            var result = assemblies
                .SelectMany(assembly => assembly.GetLoadableTypes())
                .ToList();

            Logger.Debug("Types successfully got. Types count : {0}", result.Count);
            return result;
        }

        private ICollection<Type> GetInjectableTypes(IEnumerable<Assembly> assemblies)
        {
            Logger.Debug("GetTypes started");

            var result = assemblies
                .SelectMany(assembly => assembly.GetLoadableTypes())
                .Where(IsInjectable)
                .ToList();

            Logger.Debug("Types successfully got. Types count : {0}", result.Count);
            return result;
        }

        private bool IsInjectable(Type type)
        {
            Logger.Trace("IsInjectable started for : {0}", type.FullName);
            try
            {
                return type.GetCustomAttributes(typeof(InjectableAttribute), true).Length > 0;
            }
            catch (Exception e)
            {
                Logger.Error(e, "Custom attribute get error. The target type will not be processed by IoC");
                return false;
            }
        }

        private List<Assembly> LoadAssemblies(List<string> loadingAssembliesPaths)
        {
            Logger.Debug("LoadAssemblies started");
            var loadedAssemblies = new List<Assembly>();
            loadingAssembliesPaths
                .ForEach(path =>
                    loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));
            Logger.Debug("Assemblies loaded successfully");
            return loadedAssemblies;
        }

        private List<string> FilterLoadingAssemblies(ICollection<string> referencedPaths,
            ICollection<string> loadedPaths, string moduleRegex)
        {
            Logger.Debug("FilterLoadingAssemblies started");
            var result = referencedPaths
                .Where(referenced => CheckNotContainsItem(loadedPaths, referenced) &&
                                     !MatchRegex(ExclusionsRegex, referenced) &&
                                     MatchRegex(moduleRegex, referenced))
                .ToList();
            Logger.Debug("Not loaded assemblies count : {0}", result.Count);
            return result;
        }

        private bool MatchRegex(string regex, string text)
        {
            Logger.Trace("MatchRegex started");
            return Regex.IsMatch(text, regex);
        }

        private bool CheckNotContainsItem(IEnumerable<string> elements, string item)
        {
            Logger.Trace("CheckContainsItem started");
            return !elements.Contains(item, StringComparer.InvariantCultureIgnoreCase);
        }

        private ICollection<string> GetAllLocalDllPaths()
        {
            Logger.Debug("GetAllLocalDllPaths started");
            var dllPaths = Directory
                .GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                .ToList();
            Logger.Debug("Dll paths got. Data count : {0}", dllPaths.Count);
            return dllPaths;
        }

        private ICollection<string> ExtractAssemblyPath(ICollection<Assembly> assemblies)
        {
            Logger.Debug("GetAssemblyPath started");
            var result = assemblies.Select(a => a.Location).ToList();
            Logger.Debug("Assemblies\' path extracted successfully");
            return result;
        }

        private ICollection<Assembly> GetLoadedAssemblies()
        {
            Logger.Debug("GetLoadedAssemblies started");
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            Logger.Debug("Already load to appdomain assemblies got. Assemblies count : {0}", loadedAssemblies.Count);
            return loadedAssemblies;
        }

        #endregion
    }
}