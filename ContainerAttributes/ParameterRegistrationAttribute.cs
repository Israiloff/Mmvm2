using System;

namespace Mmvm.Container.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public abstract class ParameterRegistrationAttribute : Attribute
    {
        public string Named { get; set; }
    }
}