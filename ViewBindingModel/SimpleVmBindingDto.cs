using System;

namespace Israiloff.Mmvm.Net.Mvvm.View.Services.BindingEngine.Dtos
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