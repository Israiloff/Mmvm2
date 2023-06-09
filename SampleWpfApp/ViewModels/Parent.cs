﻿using System;
using Mmvm.Logger;
using Mmvm.Mvvm.Core;

namespace SampleWpfApp.ViewModels
{
    public abstract class Parent : BaseVm, IDisposable
    {
        #region Constructors

        protected Parent(ILogger logger)
        {
            Logger = logger;
        }

        #endregion

        #region Services

        protected ILogger Logger { get; }

        #endregion

        #region INavigationNode impl

        public void Dispose()
        {
            Logger.Warn("Dispose method started");
        }

        #endregion
    }
}