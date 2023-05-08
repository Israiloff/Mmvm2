using Mmvm.Container.Attributes;
using Mmvm.Logger;
using Mmvm.Mvvm.Core;
using Mmvm.Navigation.Model;

namespace SampleWpfApp
{
    [Service(Name = nameof(MainVm), LifetimeScope = LifetimeScope.SingleInstance)]
    public class MainVm : BaseVm, INavigationNode
    {
        #region Constructor

        public MainVm(ILogger logger)
        {
            Logger = logger;
        }

        #endregion

        #region Services

        private ILogger Logger { get; }

        #endregion

        #region Dispose impl

        public void Dispose()
        {
            Logger.Info("Object disposed");
        }

        #endregion

        #region Gui properties

        private string _text = "Hello World!";

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