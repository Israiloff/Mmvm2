using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Autofac.Util;
using Mmvm.Assembly.Loader.Model;
using Mmvm.Container.Attributes;
using Mmvm.Logger;

namespace Mmvm.Assembly.Loader.Impl
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

        public LoadResult AssemblyLoadResult
        {
            get
            {
                lock (_loadAssembliesSyncObject)
                {
                    return _assemblyLoadResult;
                }
            }
            private set
            {
                lock (_loadAssembliesSyncObject)
                {
                    _assemblyLoadResult = value;
                }
            }
        }

        public ICollection<Type> LoadedTypes
        {
            get
            {
                lock (_getTypesSyncObject)
                {
                    return _loadedTypes;
                }
            }
            private set
            {
                lock (_getTypesSyncObject)
                {
                    _loadedTypes = value;
                }
            }
        }

        #endregion

        #region Private fields

        private readonly object _loadAssembliesSyncObject = new object();

        private readonly object _getTypesSyncObject = new object();

        private LoadResult _assemblyLoadResult;

        private ICollection<Type> _loadedTypes;

        #endregion

        #region IAssemblyLoader impl

        public LoadResult Load(string moduleRegex)
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
                AssemblyLoadResult = MapAssembliesToResultDto(loadedAssemblies, moduleRegex);
                return AssemblyLoadResult;
            }

            Logger.Warn("All assemblies already loaded. Operation terminated");
            AssemblyLoadResult = MapAssembliesToResultDto(loadedAssemblies, moduleRegex);
            return AssemblyLoadResult;
        }

        public ICollection<Type> GetAllLoadedTypes()
        {
            Logger.Info("GetAllLoadedTypes started");
            LoadedTypes = GetTypes(GetLoadedAssemblies());
            Logger.Debug("Loaded types\' count : {0}", LoadedTypes.Count);
            return LoadedTypes;
        }

        #endregion

        #region Private methods

        private LoadResult MapAssembliesToResultDto(IEnumerable<System.Reflection.Assembly> assemblies,
            string moduleRegex)
        {
            Logger.Debug("MapAssembliesToResult started");

            var filteredAssemblies = assemblies
                .Where(assembly => MatchRegex(moduleRegex, assembly.FullName))
                .ToList();

            var result = new LoadResult(GetInjectableTypes(filteredAssemblies),
                GetAssembliesPaths(filteredAssemblies));

            Logger.Debug("Assemblies successfully mapped to result dto");

            return result;
        }

        private ICollection<string> GetAssembliesPaths(IEnumerable<System.Reflection.Assembly> assemblies)
        {
            Logger.Debug("GetAssembliesPaths started");
            var result = assemblies.Select(assembly => assembly.Location).ToList();
            Logger.Debug("Assemblies\' paths successfully got. Count : {0}", result.Count);
            return result;
        }

        private ICollection<Type> GetTypes(IEnumerable<System.Reflection.Assembly> assemblies)
        {
            Logger.Debug("GetTypes started");

            var result = assemblies
                .SelectMany(assembly => assembly.GetLoadableTypes())
                .ToList();

            Logger.Debug("Types successfully got. Types count : {0}", result.Count);
            return result;
        }

        private ICollection<Type> GetInjectableTypes(IEnumerable<System.Reflection.Assembly> assemblies)
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

        private List<System.Reflection.Assembly> LoadAssemblies(List<string> loadingAssembliesPaths)
        {
            Logger.Debug("LoadAssemblies started");
            var loadedAssemblies = new List<System.Reflection.Assembly>();
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

        private ICollection<string> ExtractAssemblyPath(ICollection<System.Reflection.Assembly> assemblies)
        {
            Logger.Debug("GetAssemblyPath started");
            var result = assemblies.Select(a => a.Location).ToList();
            Logger.Debug("Assemblies\' path extracted successfully");
            return result;
        }

        private ICollection<System.Reflection.Assembly> GetLoadedAssemblies()
        {
            Logger.Debug("GetLoadedAssemblies started");

            var loaded = AppDomain
                .CurrentDomain
                .GetAssemblies();

            Logger.Debug("Already loaded to current domain assemblies' count : {0}", loaded.Length);

            var assemblies = Filter(
                    Directory.EnumerateFiles(
                        Directory.GetCurrentDirectory(),
                        "*.dll",
                        SearchOption.AllDirectories),
                    new[] {"."},
                    loaded.Select(assembly => "^" + assembly.GetName().Name + "*"),
                    Path.GetFileName)
                .Select(System.Reflection.Assembly.LoadFrom)
                .ToList();

            Logger.Debug("Loaded assemblies' count : {0}", assemblies.Count);

            var summary = loaded.Concat(assemblies).ToList();
            Logger.Debug("Assemblies' summary : {0}", summary.Count);

            return summary;
        }

        private static IEnumerable<T> Filter<T>(IEnumerable<T> types, IEnumerable<string> includePatterns,
            IEnumerable<string> excludePatterns, Func<T, string> targetSelector)
        {
            return types.Where(type =>
            {
                var target = targetSelector.Invoke(type);
                return includePatterns.Any(pattern => Regex.IsMatch(target, pattern))
                       && excludePatterns.All(pattern => !Regex.IsMatch(target, pattern))
                       && !Regex.IsMatch(target, ExclusionsRegex);
            });
        }

        #endregion
    }
}