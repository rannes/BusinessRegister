using BusinessRegister.Dal.Models;

namespace BusinessRegister.Dal.Exceptions
{
    /// <summary>
    /// Base exception to hold ErrorCode values.
    /// </summary>
    public interface IBrBaseException
    {
        /// <summary>
        /// Holds Result for this exception
        /// </summary>
        ResultCode Result { get; }
    }
}