using Mmvm.Navigation.Model;

namespace Mmvm.Navigation.Utilities
{
    public interface INavigationUtility
    {
        bool TryGetPageObject(string pageName, out INavigationNode navigatingPageObject);
    }
}