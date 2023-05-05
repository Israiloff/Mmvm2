using System;

namespace Israiloff.Cashbox.Component.Navigation.Model.StructuralModels
{
    //todo - rename
    public class NavigationUnit
    {
        #region Constructors

        public NavigationUnit(BranchInfo branchInfo, string currentNode, string navigatingNode,
            INavigationNode navigatingNodeModel, Action<INavigationNode> navigationCallback)
        {
            BranchInfo = branchInfo;
            CurrentNode = currentNode;
            NavigatingNode = navigatingNode;
            NavigatingNodeModel = navigatingNodeModel;
            NavigationCallback = navigationCallback;
        }

        #endregion

        #region Public properties

        public BranchInfo BranchInfo { get; }

        public string CurrentNode { get; }

        public string NavigatingNode { get; }

        public INavigationNode NavigatingNodeModel { get; }

        public Action<INavigationNode> NavigationCallback { get; }

        #endregion
    }
}