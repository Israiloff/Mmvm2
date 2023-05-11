using System.Windows.Input;
using Mmvm.Command;
using Mmvm.Container.Attributes;
using Mmvm.Logger;
using Mmvm.Navigation.Attribute;
using Mmvm.Navigation.Model;
using Mmvm.Navigation.Model.StructuralModels;
using Mmvm.Navigation.Services;
using SampleWpfApp.Pages;

namespace SampleWpfApp.ViewModels
{
    [ViewModelBinding(PageName = nameof(First))]
    [Component(Name = nameof(FirstVm), LifetimeScope = LifetimeScope.InstancePerDependency)]
    public class FirstVm : Parent, INavigationNode
    {
        #region Private fields

        private string _text = "First page";

        #endregion

        #region Constructors

        public FirstVm(ILogger logger, INavigationService navigationService) : base(logger)
        {
            NavigationService = navigationService;
        }

        #endregion

        #region Services

        private INavigationService NavigationService { get; }

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

        #region Commands

        public ICommand Command => new RelayCommand(o =>
        {
            Logger.Debug("Command started with arg : {0}", o);
            NavigationService.Forward(
                new NavigateArgs(new BranchInfo(nameof(MainVm), nameof(MainVm)), nameof(SecondVm)));
        });

        #endregion
    }
}