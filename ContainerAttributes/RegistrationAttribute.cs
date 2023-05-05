using System;
using Israiloff.Mmvm.Net.Container.Enums;

namespace Israiloff.Mmvm.Net.Container.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class RegistrationAttribute : InjectableAttribute
    {
        #region Public properties

        public LifetimeScope LifetimeScope { get; set; } = LifetimeScope.InstancePerDependency;

        public object Key { get; set; }

        public bool AsImplementedInterfaces { get; set; } = true;

        #endregion
    }
}