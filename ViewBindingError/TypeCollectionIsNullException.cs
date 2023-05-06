using System;

namespace Mmvm.View.Binding.Error
{
    public class TypeCollectionIsNullException : Exception
    {
        #region Constructors

        public TypeCollectionIsNullException(string message) : base(message)
        {
        }

        #endregion
    }
}