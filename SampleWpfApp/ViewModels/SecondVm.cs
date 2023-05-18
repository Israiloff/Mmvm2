using System.Windows.Input;
using Mmvm.Command;
using Mmvm.Container.Attributes;
using Mmvm.Logger;
using Mmvm.Navigation.Attribute;
using Mmvm.Navigation.Model.StructuralModels;
using Mmvm.Navigation.Services;
using SampleWpfApp.Pages;

namespace SampleWpfApp.ViewModels
{
    [ViewModelBinding(PageName = nameof(Second))]
    [Component(Name = nameof(SecondVm), LifetimeScope = LifetimeScope.InstancePerDependency)]
    public class SecondVm : Parent
    {
        #region Constructors

        public SecondVm(ILogger logger, INavigationService navigationService) : base(logger)
        {
            NavigationService = navigationService;
        }

        #endregion

        #region Services

        private INavigationService NavigationService { get; }

        #endregion

        ~SecondVm()
        {
            Logger.Warn("Vm disposed");
        }

        #region Private fields

        private string _title = "Second page";
        private string _text;

        #endregion

        #region Properties

        public string Title
        {
            get => _title;
            set => SetField(ref _title, value);
        }

        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }

        #endregion

        #region Commands

        public ICommand Forward => new RelayCommand(o =>
        {
            Logger.Debug("Command started with arg : {0}", o);
            NavigationService.ForwardAsync(
                new NavigateArgs(new BranchInfo(nameof(MainVm), nameof(FirstVm)), nameof(ThirdVm)));
        });

        public ICommand Back => new RelayCommand(o =>
        {
            Logger.Debug("Command started with arg : {0}", o);
            NavigationService.BackwardAsync(new BranchInfo(nameof(MainVm), nameof(FirstVm)));
        });

        #endregion
    }
}