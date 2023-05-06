using System;

namespace Mmvm.Container.Error
{
    public class CommonException : Exception
    {
        #region Constructors

        public CommonException(string message) : base(message)
        {
        }

        #endregion
    }
}