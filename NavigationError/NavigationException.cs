using System;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Mmvm.Navigation.Error
{
    public class NavigationException : Exception
    {
        public NavigationException()
        {
        }

        public NavigationException(string message) : base(message)
        {
        }
    }
}