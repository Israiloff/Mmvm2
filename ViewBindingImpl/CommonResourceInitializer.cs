using System;
using System.Collections.Generic;
using System.Windows;
using Mmvm.Container.Attributes;
using Mmvm.Logger;

namespace Mmvm.View.Binding.Impl
{
    [Service(Name = nameof(CommonResourceInitializer))]
    public class CommonResourceInitializer : IResourceInitializer
    {
        #region Constructors

        public CommonResourceInitializer(IBindingEngine bindingEngine, ILogger logger)
        {
            BindingEngine = bindingEngine;
            Logger = logger;
        }

        #endregion

        #region IResourceInitializer impl

        public void Initialize(ICollection<Type> types)
        {
            Logger.Info("Initialize method started with types count : {0}", types.Count);
            var resource = BindingEngine.GetVmBindingDictionary(types);
            Logger.Debug("Adding VM binding dictionary to merged dictionaries");
            Application.Current.Resources.MergedDictionaries.Add(resource);
        }

        #endregion

        #region Private properties

        private IBindingEngine BindingEngine { get; }

        private ILogger Logger { get; }

        #endregion
    }
}