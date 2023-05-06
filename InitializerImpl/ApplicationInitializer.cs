using System;
using System.Collections.Generic;
using Autofac;
using Israiloff.Cashbox.Component.Logger;
using Israiloff.Cashbox.Component.Logger.Impl;
using Israiloff.Cashbox.Component.Navigation.Services;
using Israiloff.Cashbox.Component.Serializer.Json;
using Israiloff.Mmvm.Net.Container.Impl.Config;
using Israiloff.Mmvm.Net.Core.Impl.Services.AssemblyLoader;
using Israiloff.Mmvm.Net.Core.Services.AssemblyLoader;
using Israiloff.Mmvm.Net.Initializer.Services.ApplicationInitializer;
using Israiloff.Mmvm.Net.Mvvm.View.Services.ResourceInitializer;

namespace Israiloff.Mmvm.Net.Initializer.Impl.Services.ApplicationInitializer
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