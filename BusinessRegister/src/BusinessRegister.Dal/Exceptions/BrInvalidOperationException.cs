using System;
using BusinessRegister.Dal.Models;

namespace BusinessRegister.Dal.Exceptions
{
    /// <inheritdoc cref="InvalidOperationException" />
    public class BrInvalidOperationException : InvalidOperationException, IBrBaseException
    {
        /// <inheritdoc />
        public ResultCode Result { get; }

        /// <summary>
        /// Exception constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="result"><see cref="ResultCode"/></param>
        public BrInvalidOperationException(string message, ResultCode result)
            : base(message)
        {
            Result = result;
        }

        /// <summary>
        /// Exception constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="result"><see cref="ResultCode"/></param>
        /// <param name="innerException">Original exception</param>
        public BrInvalidOperationException(string message, ResultCode result, Exception innerException)
            : base(message, innerException)
        {
            Result = result;
        }

        /// <summary>
        /// Exception constructor
        /// </summary>
        /// <param name="result"><see cref="ResultCode"/></param>
        public BrInvalidOperationException(ResultCode result)
        {
            Result = result;
        }
    }
}