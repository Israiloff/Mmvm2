using System.Collections.Generic;

namespace Israiloff.Cashbox.Component.Navigation.Model.StructuralModels
{
    public class NavigationTree
    {
        #region Public properties

        /// <summary>
        /// Navigation roots' map, where key is Root's name
        /// </summary>
        public Dictionary<string, RootNode> RootNodes { get; }

        #endregion

        #region Constructors

        public NavigationTree()
        {
            RootNodes = new Dictionary<string, RootNode>();
        }

        public NavigationTree(Dictionary<string, RootNode> rootNodes)
        {
            RootNodes = rootNodes;
        }

        #endregion
    }
}