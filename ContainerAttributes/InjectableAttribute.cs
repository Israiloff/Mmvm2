using System;

namespace Israiloff.Mmvm.Net.Container.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class InjectableAttribute : Attribute
    {
        #region Properties

        public string Name { get; set; }

        #endregion
    }
}