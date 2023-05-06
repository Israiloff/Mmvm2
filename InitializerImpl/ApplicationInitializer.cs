using System;
using System.Collections.Generic;
using Autofac;
using Mmvm.Assembly.Loader;
using Mmvm.Assembly.Loader.Impl;
using Mmvm.Container.Impl.Config;
using Mmvm.Logger;
using Mmvm.Logger.Impl;
using Mmvm.Navigation.Services;
using Mmvm.Serializer.Json;
using Mmvm.View.Binding;

namespace Mmvm.Initializer.Impl
{
    public class ApplicationInitializer : IApplicationInitializer
    {
        #region Private properties

        private ILogger Logger => new NLogLogger(typeof(ApplicationInitializer), new JsonSerializer());

        #endregion

        #region IApplicationInitializer impl

        public INavigationService Initialize()
        {
            var types = InitializeAppResources(string.Empty);

            var container = new ContainerConfig()
                .Config
                .GetContainer(null, types);

            var assemblyLoader = container.Resolve<IAssemblyLoader>();
            var resourceInitializer = container.Resolve<IResourceInitializer>();
            resourceInitializer.Initialize(assemblyLoader.GetAllLoadedTypes());

            return container.Resolve<INavigationService>();
        }

        #endregion

        #region Private methods

        private ICollection<Type> InitializeAppResources(string moduleRegex)
        {
            Logger.Debug("Adding VM binding dictionary to merged dictionaries");
            var types = new CommonAssemblyLoader(Logger).Load(moduleRegex)?.LoadedTypes;
            return types;
        }

        #endregion
    }
}