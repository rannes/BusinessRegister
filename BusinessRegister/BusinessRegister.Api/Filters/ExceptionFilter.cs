using System;
using System.Net;
using BusinessRegister.Dal.Exceptions;
using BusinessRegister.Dal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BusinessRegister.Api.Filters
{
    /// <summary>
    /// Handles exceptions - converts them into error struct
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        /// <inheritdoc />
        public void OnException(ExceptionContext context)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;

            if (context.Exception is BrBaseException baseException)
                statusCode = baseException.StatusCode;

            context.Result = new ObjectResult(Transform(context.Exception));
            context.HttpContext.Response.StatusCode = statusCode;
        }

        /// <summary>
        /// Transforms exception into agreed format
        /// </summary>
        /// <param name="exception">input exeption</param>
        public static Answer Transform(Exception exception)
        {
            switch (exception)
            {
                case BaseRequestException baseRequestExeption:
                    var msg = baseRequestExeption.Result.ToString();

                    if (string.IsNullOrEmpty(msg))
                    {
                        msg = exception.Message;
                    }

                    return new Answer
                    {
                        Success = false,
                        ResultMsg = baseRequestExeption.Result == ResultCode.ServerError ? exception.ToString() : msg
                    };

                case BrBaseException brException:
                    return new Answer
                    {
                        ResultMsg = ((ResultCode)brException.ErrorCode).ToString(),
                        Success = false
                    };
                default:
                    return new Answer
                    {
                        ResultMsg = exception.ToString(),
                        Success = false
                    };
            }
        }
    }
}