namespace Israiloff.Cashbox.Component.Navigation.Model.StructuralModels
{
    public abstract class BaseModel
    {
        #region Constructors

        protected BaseModel(string name)
        {
            Name = name;
        }

        #endregion

        #region Public properties

        public string Name { get; }

        public bool IsActive { get; set; }

        #endregion
    }
}