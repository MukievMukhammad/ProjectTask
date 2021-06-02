using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TaskTracker.ActionFilter
{
    public enum ValidationResult
    {
        View,
        Json
    }

    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        private readonly ValidationResult _result;

        public ValidationFilterAttribute(ValidationResult result
            = ValidationResult.Json)
        {
            _result = result;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                if (_result == ValidationResult.Json)
                {
                    context.Result = new ValidationFailedResult(context.ModelState);
                }
            }
        }
    }

    public class ValidationFailedResult : JsonResult
    {
        public readonly List<ModelErrorCollection> Errors;
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(modelState.Select(x => new
            {
                x.Key,
                ValidationState = x.Value.ValidationState.ToString(),
                x.Value.Errors
            }).ToList())
        {
            Errors = modelState.Select(x => x.Value.Errors).ToList();
        }

        public override void ExecuteResult(ActionContext context)
        {
            base.ExecuteResult(context);
            SetStatusCodeAndHeaders(context.HttpContext);
        }

        internal static void SetStatusCodeAndHeaders(HttpContext context)
        {
            context.Response.StatusCode = 422;
            context.Response.Headers.Add("X-Status-Reason", "Validation failed");
        }
    }
}