using Israiloff.Cashbox.Component.Navigation.Model;

namespace Israiloff.Cashbox.Component.Navigation.Utilities
{
    public interface INavigationUtility
    {
        bool TryGetPageObject(string pageName, out INavigationNode navigatingPageObject);
    }
}