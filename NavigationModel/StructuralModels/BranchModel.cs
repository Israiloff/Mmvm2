using System;

namespace Mmvm.Navigation.Model.StructuralModels
{
    //todo - rename
    public class BranchModel : BranchInfo
    {
        #region Constructors

        public BranchModel(string parentNodeName, string branchName, Action<INavigationNode> navigationCallback)
            : base(parentNodeName, branchName)
        {
            NavigationCallback = navigationCallback;
        }

        #endregion

        #region Public properties

        public Action<INavigationNode> NavigationCallback { get; }

        #endregion
    }
}