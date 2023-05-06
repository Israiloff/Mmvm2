using Newtonsoft.Json;

namespace Mmvm.Navigation.Model.StructuralModels
{
    public class Node
    {
        #region Constructors

        public Node(string name, INavigationNode value)
        {
            Name = name;
            Value = value;
        }

        #endregion

        #region Public properties

        public string Name { get; }

        [JsonIgnore] public INavigationNode Value { get; }

        #endregion
    }
}