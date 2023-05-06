using Israiloff.Cashbox.Component.Navigation.Services;

namespace Israiloff.Mmvm.Net.Initializer.Services.ApplicationInitializer
{
    public interface IApplicationInitializer
    {
        INavigationService Initialize();
    }
}