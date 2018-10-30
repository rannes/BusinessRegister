using System;
using BusinessRegister.Dal.Models;

namespace BusinessRegister.Dal.Exceptions
{
    /// <inheritdoc cref="ArgumentException" />
    public class BrArgumentException : ArgumentException, IBrBaseException
    {
        /// <inheritdoc />
        public ResultCode Result { get; }

        /// <summary>
        /// Exception constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="argumentName">Argument that caused the exception</param>
        /// <param name="result"><see cref="ResultCode"/></param>
        public BrArgumentException(string message, string argumentName, ResultCode result)
            : base(message, argumentName)
        {
            Result = result;
        }

        /// <summary>
        /// Exception constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="argumentName">Argument that caused the exception</param>
        /// <param name="result"><see cref="ResultCode"/></param>
        /// <param name="innerException">Original exception</param>
        public BrArgumentException(string message, string argumentName, ResultCode result, Exception innerException)
            : base(message, argumentName, innerException)
        {
            Result = result;
        }

        /// <summary>
        /// Exception constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="result"><see cref="ResultCode"/></param>
        /// <param name="innerException">Original exception</param>
        public BrArgumentException(string message, ResultCode result, Exception innerException)
            : base(message, innerException)
        {
            Result = result;
        }

        /// <summary>
        /// Exception constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="result"><see cref="ResultCode"/></param>
        public BrArgumentException(string message, ResultCode result)
            : base(message)
        {
            Result = result;
        }

        /// <summary>
        /// Exception constructor
        /// </summary>
        /// <param name="result"><see cref="ResultCode"/></param>
        public BrArgumentException(ResultCode result)
        {
            Result = result;
        }
    }
}