using Mmvm.Container.Attributes;
using Mmvm.Logger;
using Mmvm.Mvvm.Core;
using Mmvm.Navigation.Model;
using Mmvm.Navigation.Model.StructuralModels;
using Mmvm.Navigation.Services;
using Mmvm.Synchronizer;
using SampleWpfApp.ViewModels;

namespace SampleWpfApp
{
    [Component(Name = nameof(MainVm), LifetimeScope = LifetimeScope.SingleInstance)]
    public class MainVm : BaseVm
    {
        #region Constructor

        public MainVm(ILogger logger, INavigationService navigationService, ISynchronizer synchronizer)
        {
            Logger = logger;
            NavigationService = navigationService;
            Synchronizer = synchronizer;
            navigationService.CreateBranch(new BranchModel(nameof(MainVm), nameof(FirstVm), ChangePage));
            NavigationService.ForwardAsync(new NavigateArgs(new BranchInfo(nameof(MainVm), nameof(FirstVm)),
                nameof(FirstVm)));
        }

        #endregion

        #region Services

        private ILogger Logger { get; }

        private INavigationService NavigationService { get; }

        private ISynchronizer Synchronizer { get; }

        #endregion

        #region Private fields

        private string _text = "Main Page";

        private INavigationNode _currentPage;

        #endregion

        #region Gui properties

        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }

        public INavigationNode CurrentPage
        {
            get => _currentPage;
            private set => SetField(ref _currentPage, value);
        }

        public void ChangePage(INavigationNode node)
        {
            Logger.Debug("{0} started for : {1}", nameof(ChangePage), node.GetType().FullName);
            Synchronizer.Sync(() =>
            {
                var current = CurrentPage;
                CurrentPage = node;
                current?.Dispose();
            });
        }

        #endregion
    }
}