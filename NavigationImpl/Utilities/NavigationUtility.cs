using Autofac;
using Israiloff.Cashbox.Component.Logger;
using Israiloff.Cashbox.Component.Navigation.Model;
using Israiloff.Cashbox.Component.Navigation.Utilities;

namespace Israiloff.Cashbox.Component.Navigation.Impl.Utilities
{
    public class NavigationUtility : INavigationUtility
    {
        #region Constructors

        public NavigationUtility(ILogger logger, IComponentContext container)
        {
            Logger = logger;
            Container = container;
        }

        #endregion

        #region INavigationUtility impl

        public bool TryGetPageObject(string pageName, out INavigationNode navigatingPageObject)
        {
            Logger.Debug("GetPageObject method started");

            Logger.Debug("Searching for requested page object");
            navigatingPageObject = Container.ResolveOptionalNamed<INavigationNode>(pageName);

            if (navigatingPageObject == null)
            {
                Logger.Warn("Requested page not found. Operation terminated");
                return false;
            }

            return true;
        }

        #endregion

        #region Private properties

        private ILogger Logger { get; }
        private IComponentContext Container { get; }

        #endregion
    }
}