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
        #region Constructors

        public FirstVm(ILogger logger, INavigationService navigationService) : base(logger)
        {
            NavigationService = navigationService;
        }

        #endregion

        #region Services

        private INavigationService NavigationService { get; }

        #endregion

        #region Commands

        public ICommand Forward => new RelayCommand(o =>
        {
            Logger.Debug("Command started with arg : {0}", o);
            NavigationService.ForwardAsync(
                new NavigateArgs(new BranchInfo(nameof(MainVm), nameof(FirstVm)), nameof(SecondVm)));
        });

        #endregion

        #region Private fields

        private string _title = "First page";
        private string _text;

        #endregion

        #region Properties

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

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