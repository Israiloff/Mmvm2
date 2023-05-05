using Israiloff.Cashbox.Component.Navigation.Model.StructuralModels;

namespace Israiloff.Cashbox.Component.Navigation.Model.EventArgs
{
    public class BranchDisabledEventArgs
    {
        #region Constructors

        public BranchDisabledEventArgs(BranchInfo branchInfo)
        {
            BranchInfo = branchInfo;
        }

        #endregion

        #region Properties

        public BranchInfo BranchInfo { get; }

        #endregion
    }
}