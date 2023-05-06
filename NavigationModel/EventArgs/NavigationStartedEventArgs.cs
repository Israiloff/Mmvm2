using Mmvm.Navigation.Model.Enums;
using Mmvm.Navigation.Model.StructuralModels;

namespace Mmvm.Navigation.Model.EventArgs
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