using System;

namespace Mmvm.View.Binding.Model
{
    public class SimpleVmBinding
    {
        #region Constructors

        public SimpleVmBinding(Type view, Type viewModel)
        {
            View = view;
            ViewModel = viewModel;
        }

        #endregion

        #region Properties

        public Type View { get; }

        public Type ViewModel { get; }

        #endregion
    }
}