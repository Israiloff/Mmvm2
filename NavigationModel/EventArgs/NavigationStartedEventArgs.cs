using Israiloff.Cashbox.Component.Navigation.Model.Enums;
using Israiloff.Cashbox.Component.Navigation.Model.StructuralModels;

namespace Israiloff.Cashbox.Component.Navigation.Model.EventArgs
{
    public class NavigationStartedEventArgs : System.EventArgs
    {
        #region Constructors

        public NavigationStartedEventArgs(BranchInfo branchInfo, string currentPageName, Direction direction)
        {
            BranchInfo = branchInfo;
            CurrentPageName = currentPageName;
            Direction = direction;
        }

        #endregion

        #region Properties

        public BranchInfo BranchInfo { get; }

        public string CurrentPageName { get; }

        public Direction Direction { get; }

        #endregion
    }
}