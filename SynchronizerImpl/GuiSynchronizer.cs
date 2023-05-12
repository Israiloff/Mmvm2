using System;
using System.Windows;
using System.Windows.Threading;
using Mmvm.Container.Attributes;
using Mmvm.Logger;

namespace Mmvm.Synchronizer.Impl
{
    [Service(Name = nameof(GuiSynchronizer), LifetimeScope = LifetimeScope.SingleInstance)]
    public class GuiSynchronizer : ISynchronizer
    {
        public GuiSynchronizer(ILogger logger)
        {
            Logger = logger;
        }

        private ILogger Logger { get; }

        public void Sync(Action action)
        {
            Logger.Trace("{0} started for : {1}", nameof(Sync), action?.Method.Name);

            if (action == null)
            {
                Logger.Error("{0} method's argument is null", nameof(Sync));
                throw new ArgumentNullException(nameof(action));
            }

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, action);
            Logger.Trace("{0} successfully finished for : {1}", nameof(Sync), action.Method.Name);
        }
    }
}