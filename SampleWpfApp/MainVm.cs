using Mmvm.Container.Attributes;
using Mmvm.Logger;
using Mmvm.Mvvm.Core;
using Mmvm.Navigation.Model;
using Mmvm.Navigation.Model.StructuralModels;
using Mmvm.Navigation.Services;
using SampleWpfApp.ViewModels;

namespace SampleWpfApp
{
    [Component(Name = nameof(MainVm), LifetimeScope = LifetimeScope.SingleInstance)]
    public class MainVm : BaseVm, INavigationNode
    {
        #region Constructor

        public MainVm(ILogger logger, INavigationService navigationService
            // , GuiSynchronizer synchronizer
        )
        {
            Logger = logger;
            NavigationService = navigationService;
            // Synchronizer = synchronizer;
            navigationService.CreateBranch(new BranchModel(nameof(MainVm), nameof(FirstVm), ChangePage));
            NavigationService.ForwardAsync(new NavigateArgs(new BranchInfo(nameof(MainVm), nameof(FirstVm)),
                nameof(FirstVm)));
        }

        #endregion

        #region Dispose impl

        public void Dispose()
        {
            Logger.Info("Object disposed");
        }

        #endregion

        #region Services

        private ILogger Logger { get; }

        private INavigationService NavigationService { get; }

        // private GuiSynchronizer Synchronizer { get; }

        #endregion

        #region Private fields

        private string _text = "Hello World!";

        private INavigationNode _currentPage;

        #endregion

        #region Gui properties

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        public INavigationNode CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }

        public void ChangePage(INavigationNode node)
        {
            Logger.Debug("{0} started for : {1}", nameof(ChangePage), node.GetType().FullName);
            // Synchronizer.Sync(() =>
            // {
            var current = CurrentPage;
            CurrentPage = node;
            current?.Dispose();
            // });
        }

        #endregion
    }
}