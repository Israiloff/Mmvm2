using Mmvm.Container.Attributes;
using Mmvm.Logger;
using Mmvm.Navigation.Attribute;
using Mmvm.Navigation.Model;
using SampleWpfApp.Pages;

namespace SampleWpfApp.ViewModels
{
    [ViewModelBinding(PageName = nameof(Third))]
    [Component(Name = nameof(ThirdVm), LifetimeScope = LifetimeScope.InstancePerDependency)]
    public class ThirdVm : Parent, INavigationNode
    {
        #region Private fields

        private string _text = "Third page";

        #endregion

        #region Constructors

        public ThirdVm(ILogger logger) : base(logger)
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