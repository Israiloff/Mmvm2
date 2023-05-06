using Mmvm.Navigation.Model.StructuralModels;

namespace Mmvm.Navigation.Model.EventArgs
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