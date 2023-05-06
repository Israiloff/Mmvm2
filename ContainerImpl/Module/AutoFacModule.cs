using System;
using System.Collections.Generic;
using Autofac;
using Mmvm.Container.Impl.Services;

namespace Mmvm.Container.Impl.Module
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