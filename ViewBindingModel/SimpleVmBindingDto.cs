using System;

namespace Mmvm.View.Binding.Model
{
    public class SimpleVmBindingDto
    {
        #region Constructors

        public SimpleVmBindingDto(Type view, Type viewModel)
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