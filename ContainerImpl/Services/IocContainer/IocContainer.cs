using System;
using System.Collections.Generic;
using Autofac;
using Mmvm.Container.Attributes;
using Mmvm.Container.Configs;
using Mmvm.Container.Error;
using Mmvm.Container.Services.IocContainer;
using Mmvm.Logger;
using Mmvm.Serializer;

namespace Mmvm.Container.Impl.Services.IocContainer
{
    [Service(Name = nameof(IocContainer))]
    public class IocContainer : IIocContainer
    {
        #region Constructors

        public IocContainer(IContainerConfig<IContainer> containerConfig, ILogger logger,
            ISerializer serializer)
        {
            Container = containerConfig.GetContainer(null, new List<Type>());
            Logger = logger;
            Serializer = serializer;
        }

        #endregion

        #region Private properties

        private IContainer Container { get; }

        private ILogger Logger { get; }

        private ISerializer Serializer { get; }

        private string AdditionalErrorMessage => "Make sure it was implemented and marked with one of implementation " +
                                                 "attributes (ServiceAttribute, ComponentAttribute, etc)";

        #endregion

        #region IIocContainer impl

        public TObject Resolve<TObject>()
        {
            Logger.Trace("Resolve started for type : {0}", typeof(TObject).Name);

            try
            {
                return Container.Resolve<TObject>();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Resolve method failed");
                throw new CommonException($"Ioc container can't resolve type : {typeof(TObject).Name}. " +
                                          "Make sure it was implemented and marked with one of implementation " +
                                          "attributes (ServiceAttribute, ComponentAttribute, etc)");
            }
        }

        public TObject Resolve<TObject>(string name)
        {
            Logger.Trace("Resolve started for type : {0}, with name : {1}", typeof(TObject).Name, name);
            try
            {
                return Container.ResolveNamed<TObject>(name);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Resolve method failed");
                throw new CommonException($"Ioc container can't resolve type : {typeof(TObject).Name}, " +
                                          $"with name : {name}. {AdditionalErrorMessage}");
            }
        }

        public TObject Resolve<TObject>(object key)
        {
            Logger.Trace("Resolve started for type : {0}, with key : {1}", typeof(TObject).Name, key);

            try
            {
                return Container.ResolveKeyed<TObject>(key);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Resolve method failed");
                throw new CommonException($"Ioc container can't resolve type : {typeof(TObject).Name}, " +
                                          $"with key : {Serializer.Serialize(key)}. {AdditionalErrorMessage}");
            }
        }

        public TObject ResolveOptional<TObject>() where TObject : class
        {
            Logger.Trace("Resolve started for type : {0}", typeof(TObject).Name);
            return Container.ResolveOptional<TObject>();
        }

        public TObject ResolveOptional<TObject>(string name) where TObject : class
        {
            Logger.Trace("Resolve started for type : {0}, with name : {1}", typeof(TObject).Name, name);
            return Container.ResolveOptionalNamed<TObject>(name);
        }

        public TObject ResolveOptional<TObject>(object key) where TObject : class
        {
            Logger.Trace("Resolve started for type : {0}, with key : {1}", typeof(TObject).Name, key);
            return Container.ResolveOptionalKeyed<TObject>(key);
        }

        #endregion
    }
}