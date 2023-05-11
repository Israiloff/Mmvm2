using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Mmvm.Container.Attributes;
using Registration =
    Autofac.Builder.IRegistrationBuilder<object, Autofac.Builder.ConcreteReflectionActivatorData,
        Autofac.Builder.SingleRegistrationStyle>;

namespace Mmvm.Container.Impl.Services
{
    public static class ContainerBuilderService
    {
        public static void RegisterAttributedClasses(this ContainerBuilder builder, ICollection<Type> types)
        {
            types
                .Where(type => !type.IsInterface && type.GetCustomAttribute<InjectableAttribute>() != null)
                .ToList()
                .ForEach(type =>
                {
                    var registration = builder.RegisterType(type);
                    ConfigureParameters(registration, type);
                    ResolveRegistrationByAttributeType(registration, type);
                });
        }

        private static void ResolveRegistrationByAttributeType(Registration registration, Type type)
        {
            if (type.GetCustomAttribute<RegistrationAttribute>() != null)
            {
                var attribute = type.GetCustomAttribute<RegistrationAttribute>();
                SetLifestyle(registration, attribute.LifetimeScope);
                RegisterAs(registration, attribute, type);
                return;
            }

            var parentInterfaces = type.GetInterfaces().ToList();

            if (parentInterfaces.Any())
            {
                parentInterfaces.ForEach(parentInterface =>
                    RegisterNamed(registration, type.GetCustomAttribute<InjectableAttribute>(), parentInterface));
                registration.AsImplementedInterfaces();
                return;
            }

            RegisterNamed(registration, type.GetCustomAttribute<InjectableAttribute>(), type);
            registration.AsSelf();
        }

        private static void ConfigureParameters(Registration registration, Type type)
        {
            var attributedParameters = type.GetTypeInfo().GetConstructors()
                .SelectMany(c => c.GetParameters())
                .Select(p => new {info = p, attribute = p.GetCustomAttribute<ParameterRegistrationAttribute>()})
                .Where(p => p.attribute != null);

            foreach (var parameter in attributedParameters)
            {
                if (parameter.attribute.Named != null)
                    registration.WithParameter((p, c) => p == parameter.info,
                        (p, c) => c.ResolveNamed(parameter.attribute.Named, parameter.info.ParameterType));
            }
        }

        private static void SetLifestyle(Registration registration, LifetimeScope lifetimeScope)
        {
            switch (lifetimeScope)
            {
                case LifetimeScope.InstancePerDependency:
                    registration.InstancePerDependency();
                    break;
                case LifetimeScope.InstancePerLifetimeScope:
                    registration.InstancePerLifetimeScope();
                    break;
                case LifetimeScope.SingleInstance:
                    registration.SingleInstance();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetimeScope));
            }
        }

        private static void RegisterAs(Registration registration, RegistrationAttribute attribute, Type type)
        {
            if (attribute.AsImplementedInterfaces)
            {
                type.GetInterfaces()
                    .ToList()
                    .ForEach(parentInterface =>
                    {
                        RegisterNamed(registration, attribute, parentInterface);
                        RegisterKeyed(registration, attribute, parentInterface);
                    });

                registration.AsImplementedInterfaces();
            }
            else
            {
                RegisterNamed(registration, attribute, type);
                RegisterKeyed(registration, attribute, type);
                registration.AsSelf();
            }
        }

        private static void RegisterNamed(Registration registration, InjectableAttribute attribute, Type asType)
        {
            if (attribute.Name != null)
                registration.Named(attribute.Name, asType);
        }

        private static void RegisterKeyed(Registration registration, RegistrationAttribute attribute, Type asType)
        {
            if (attribute.Key != null)
                registration.Keyed(attribute.Key, asType);
        }
    }
}