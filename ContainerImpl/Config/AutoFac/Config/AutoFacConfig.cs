using System;
using System.Collections.Generic;
using Autofac;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Mmvm.Container.Configs;
using Mmvm.Container.Impl.Module;
using Mmvm.Logger.Impl;
using Mmvm.Mapper.Profile;

namespace Mmvm.Container.Impl.Config.AutoFac.Config
{
    public class AutoFacConfig : IContainerConfig<IContainer>
    {
        #region Private properties

        private IContainer Container { get; set; }

        #endregion

        #region IContainerConfig impl

        public IContainer GetContainer(IMapperProfile mapperProfile, ICollection<Type> registrationTypes)
        {
            if (Container != null)
            {
                return Container;
            }

            var builder = GetContainerBuilder(mapperProfile, registrationTypes);
            Container = builder.Build();
            return Container;
        }

        #endregion

        #region Private methods

        private ContainerBuilder GetContainerBuilder(IMapperProfile mapperProfile, ICollection<Type> registrationTypes)
        {
            AutoFacModule.Types = registrationTypes;
            var builder = new ContainerBuilder();
            builder.RegisterModule<NlogModule>();
            builder.RegisterModule<AutoFacModule>();
            builder.RegisterInstance<IContainerConfig<IContainer>>(this);

            if (mapperProfile != null)
            {
                builder.Register(context =>
                        new MapperConfiguration(configurationExpression =>
                        {
                            configurationExpression.AddProfile(mapperProfile.GetProfileType());
                            configurationExpression.AddExpressionMapping();
                        }))
                    .AsSelf()
                    .SingleInstance();
            }

            builder.Register(c =>
                {
                    var context = c.Resolve<IComponentContext>();
                    var config = context.Resolve<MapperConfiguration>();
                    return config.CreateMapper(context.Resolve);
                })
                .As<IMapper>();

            return builder;
        }

        #endregion
    }
}