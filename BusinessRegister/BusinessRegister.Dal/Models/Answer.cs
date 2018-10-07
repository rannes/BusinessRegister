using System;
using System.Collections.Generic;

namespace BusinessRegister.Dal.Models
{
    /// <summary>
    /// API result with Data List field
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataListAnswer<T> : Answer
    {
        /// <summary>
        /// API result with Data list answer
        /// </summary>
        public IEnumerable<T> Data { get; set; }
    }

    /// <summary>
    /// API result with a Data field
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataAnswer<T> : Answer
    {
        /// <summary>
        /// Templated data answer
        /// </summary>
        public T Data { get; set; }
    }


    /// <summary>
    /// Base class for Api results
    /// </summary>
    public class Answer
    {
        /// <summary>
        /// Return if the request was successful.
        /// Default value is True, but exception can override it.
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Action timestamp
        /// </summary>
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Result key.
        /// </summary>
        public string ResultMsg { get; set; } = ResultCode.Ok.ToString();
    }
}