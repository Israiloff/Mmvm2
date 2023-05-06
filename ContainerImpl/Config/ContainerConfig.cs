using Autofac;
using Mmvm.Container.Configs;
using Mmvm.Container.Impl.Config.AutoFac.Config;

namespace Mmvm.Container.Impl.Config
{
    public class ContainerConfig
    {
        #region Private fields

        private IContainerConfig<IContainer> _config;

        #endregion

        #region Public properties

        public IContainerConfig<IContainer> Config
        {
            get => _config;
            set => _config = value ?? _config;
        }

        #endregion

        #region Constructors

        public ContainerConfig()
        {
            Config = new AutoFacConfig();
        }

        public ContainerConfig(IContainerConfig<IContainer> config)
        {
            _config = config ?? new AutoFacConfig();
        }

        #endregion
    }
}