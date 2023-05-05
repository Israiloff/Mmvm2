using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Core.Resolving.Pipeline;

namespace Israiloff.Cashbox.Component.Logger.Impl
{
    public class NlogModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NLogLogger>().As<ILogger>();
        }

        protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry,
            IComponentRegistration registration)
        {
            var type = registration.Activator.LimitType;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return;
            }

            registration.PipelineBuilding += (sender, builder) =>
            {
                builder.Use(PipelinePhase.ParameterSelection, MiddlewareInsertionMode.StartOfPhase,
                    (context, action) =>
                    {
                        var logParameter = new ResolvedParameter(
                            (p, c) => p.ParameterType == typeof(ILogger),
                            (p, c) => c.Resolve<ILogger>(TypedParameter.From(type)));
                        context.ChangeParameters(context.Parameters.Union(new[] { logParameter }).ToList());
                        action(context);
                    });
            };
        }
    }
}