using System;
using System.Net;
using BusinessRegister.Dal.Models;

namespace BusinessRegister.Dal.Exceptions
{
    /// <summary>
    /// Request base exception usually for general server errors.
    /// </summary>
    public class BaseRequestException : Exception, IBrBaseException
    {
        /// <summary>
        /// Http status code
        /// </summary>
        public virtual int StatusCode { get; } = (int)HttpStatusCode.InternalServerError;

        /// <summary>
        /// A short, human-readable summary of the problem
        /// </summary>
        public virtual string Title { get; } = "There has been an error when processing your request.";

        /// <inheritdoc />
        public ResultCode Result { get; set; }

        /// <summary>
        /// Error code presented as a number in string. Look supported error codes from <see cref="ErrorCode"/> enum.
        /// </summary>
        public int ResultId => (int)Result;

        /// <summary>
        /// Base constructor
        /// </summary>
        public BaseRequestException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="result">Error code. See <see cref="Result"/> for supported codes</param>
        public BaseRequestException(string message, ResultCode result)
            : base(message)
        {
            Result = result;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="result">Error code</param>
        /// <param name="innerException"></param>
        public BaseRequestException(string message, ResultCode result, Exception innerException)
            : base(message, innerException)
        {
            Result = result;
        }
    }
}