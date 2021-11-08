using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SmartWallit.Core.Entities;
using SmartWallit.Core.Exceptions;
using SmartWallit.Core.Interfaces;
using SmartWallit.Core.Models;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SmartWallit.MIddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private ILogRepository _logRepository;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, ILogRepository logRepository)
        {
            _logRepository = logRepository;

            context.Response.ContentType = "application/json";

            try
            {
                await next(context);
            }
            catch (CustomException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, CustomException exception)
        {
            var errorResponse = new ErrorDetails()
            {
                Message = exception.Message,
                StatusCode = (int)exception.StatusCode,
                StackTrace = exception.StackTrace,
                Field = exception.Field
            };

            await LogException(context, errorResponse);

            context.Response.StatusCode = (int)exception.StatusCode;

            await context.Response.WriteAsync(errorResponse.ToString());

        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var errorResponse = new ErrorDetails()
            {
                Message = ex.Message,
                StatusCode = (int)HttpStatusCode.InternalServerError,
                StackTrace = ex.StackTrace
            };

            await LogException(context, errorResponse);

            // We want to log exception message but not return in api
            errorResponse.Message = "Error occured while processing this request. Please try again.";

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(errorResponse.ToString());
        }

        private async Task LogException(HttpContext context, ErrorDetails errorResponse)
        {
            var logEntity = new LogEntity
            {
                ServerName = Environment.MachineName,
                ApiMethodName = context.GetRouteValue("action").ToString(),
                ExceptionMessage = errorResponse.Message,
                StackTrace = errorResponse.StackTrace,
                HttpMethod = context.Request.Method,
                HttpStatusCode = errorResponse.StatusCode.ToString(),
                UserId = context.User?.Claims.FirstOrDefault(c => c.Type == "userId").Value,
            };

            await _logRepository.Log(logEntity);
        }

    }
}
