using System.Collections.Generic;

namespace Mmvm.Navigation.Model.StructuralModels
{
    public class RootNode : BaseModel
    {
        #region Public properties

        /// <summary>
        /// Navigation branches' map, where key is branch's name
        /// </summary>
        public Dictionary<string, Branch> Branches { get; }

        #endregion

        #region Constructors

        public RootNode(string name, Dictionary<string, Branch> branches, bool isActive = true) : base(name)
        {
            Branches = branches;
            IsActive = isActive;
        }

        public RootNode(string name) : base(name)
        {
            Branches = new Dictionary<string, Branch>();
            IsActive = true;
        }

        #endregion
    }
}