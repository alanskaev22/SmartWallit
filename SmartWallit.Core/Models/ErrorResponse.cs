using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartWallit.Core.Models
{
    public class ErrorResponse
    {
        public List<ErrorDetails> Errors { get; set; } = new List<ErrorDetails>();
        public ErrorResponse() { }
        public ErrorResponse(ModelStateDictionary modelState)
        {
            Errors = modelState.Keys.SelectMany(key => modelState[key].Errors.Select(x => new ErrorDetails
            {
                StatusCode = 400,
                Field = key.Length > 0 ? key.ToLower()[0] + key[1..] : key,
                Message = x.ErrorMessage
            })).ToList();
        }
    }
}
