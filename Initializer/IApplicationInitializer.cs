using Mmvm.Navigation.Services;

namespace Mmvm.Initializer
{
    public interface IApplicationInitializer
    {
        INavigationService Initialize();
    }
}