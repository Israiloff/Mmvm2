using Mmvm.Navigation.Model.StructuralModels;

namespace Mmvm.Navigation.Model.EventArgs
{
    public class AfterNavigationEventArgs : System.EventArgs
    {
        #region Constructors

        public AfterNavigationEventArgs(string navigatedPageName, BranchInfo branchInfo)
        {
            NavigatedPageName = navigatedPageName;
            BranchInfo = branchInfo;
        }

        #endregion

        #region Properties

        public string NavigatedPageName { get; }

        public BranchInfo BranchInfo { get; }

        #endregion
    }
}