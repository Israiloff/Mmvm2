using System;
using System.Collections.Generic;
using Autofac;
using Israiloff.Mmvm.Net.Container.Impl.Services;

namespace Israiloff.Mmvm.Net.Container.Impl.Module
{
    public class AutoFacModule : Autofac.Module
    {
        public static ICollection<Type> Types { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAttributedClasses(Types);
        }
    }
}