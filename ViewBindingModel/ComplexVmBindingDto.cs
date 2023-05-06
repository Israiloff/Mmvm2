using System.Windows;

namespace Mmvm.View.Binding.Model
{
    public class ComplexVmBindingDto
    {
        #region Constructors

        public ComplexVmBindingDto(DataTemplateKey key, DataTemplate value)
        {
            Key = key;
            Value = value;
        }

        #endregion

        #region Properties

        public DataTemplateKey Key { get; }

        public DataTemplate Value { get; }

        #endregion
    }
}