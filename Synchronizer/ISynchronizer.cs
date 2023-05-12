using System;

namespace Mmvm.Synchronizer
{
    public interface ISynchronizer
    {
        void Sync(Action action);
    }
}