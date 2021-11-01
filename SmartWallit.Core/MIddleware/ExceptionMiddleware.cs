using Microsoft.AspNetCore.Http;
using SmartWallit.Core.Exceptions;
using SmartWallit.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmartWallit.Core.MIddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            try
            {
                await next(context);
            }
            catch (CustomException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (ValidationException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, CustomException exception)
        {
            var errorResponse = new ErrorDetails()
            {
                Message = exception.Message,
                StatusCode = (int)exception.StatusCode,
                StackTrace = exception.StackTrace,
                Field = exception.Field
            };

            // Log errorResponse somewhere
            Console.WriteLine(errorResponse.StackTrace);

            context.Response.StatusCode = (int)exception.StatusCode;


            return context.Response.WriteAsync(errorResponse.ToString());
        }

        private Task HandleExceptionAsync(HttpContext context, ValidationException ex)
        {
            var errorResponse = new ErrorDetails()
            {
                Message = ex.Message,
                StatusCode = (int)HttpStatusCode.InternalServerError,
                StackTrace = ex.StackTrace
            };

            // Log errorResponse somewhere

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(errorResponse.ToString());
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var errorResponse = new ErrorDetails()
            {
                Message = $"Error occured while processing this request. Please try again.",
                StatusCode = (int)HttpStatusCode.InternalServerError,
                StackTrace = ex.StackTrace
            };

            // Log errorResponse somewhere

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(errorResponse.ToString());
        }
    }
}
