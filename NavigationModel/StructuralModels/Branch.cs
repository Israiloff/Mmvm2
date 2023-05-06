using System;
using System.Collections.Generic;

namespace Mmvm.Navigation.Model.StructuralModels
{
    public class Branch : BaseModel
    {
        #region Public properties

        public Stack<Node> Nodes { get; }

        public Action<INavigationNode> Callback { get; }

        #endregion

        #region Constructors

        public Branch(string name, Action<INavigationNode> callback) : base(name)
        {
            Callback = callback;
            Nodes = new Stack<Node>();
            IsActive = true;
        }

        public Branch(string name, Stack<Node> nodes, Action<INavigationNode> callback,
            bool isActive = true) : base(name)
        {
            Nodes = nodes;
            Callback = callback;
            IsActive = isActive;
        }

        #endregion
    }
}