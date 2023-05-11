using Mmvm.Container.Attributes;
using Mmvm.Logger;
using Mmvm.Navigation.Attribute;
using Mmvm.Navigation.Model;
using SampleWpfApp.Pages;

namespace SampleWpfApp.ViewModels
{
    [ViewModelBinding(PageName = nameof(Second))]
    [Component(Name = nameof(SecondVm), LifetimeScope = LifetimeScope.InstancePerDependency)]
    public class SecondVm : Parent, INavigationNode
    {
        #region Private fields

        private string _text = "Second page";

        #endregion

        #region Constructors

        public SecondVm(ILogger logger) : base(logger)
        {
        }

        #endregion

        #region Properties

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}