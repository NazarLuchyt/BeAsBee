using System;
using System.Net;
using System.Text;
using BeAsBee.Domain.Common.Exceptions;
using BeAsBee.Domain.Resources;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace BeAsBee.API.Filters {
    public class GlobalExceptionFilter : IExceptionFilter {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter ( ILogger<GlobalExceptionFilter> logger ) {
            _logger = logger;
        }

        public void OnException ( ExceptionContext context ) {
            var error = new StringBuilder( string.Format( Translations.EXCEPTION_OCCURED_WHILE_PROCESSING_REQUEST, context.HttpContext.Request.GetDisplayUrl() ) );
            error.AppendLine( string.Format( Translations.EXEPTION_MESSAGE, context.Exception.Message ) );
            error.AppendLine( string.Format( Translations.STACK_TRACE, context.Exception.StackTrace ) );
            if ( context.Exception.InnerException != null ) {
                error.AppendLine( string.Format( Translations.INNER_EXCEPTION, context.Exception.InnerException.Message ) );
            }

            _logger.LogError( error.ToString() );

            context.ExceptionHandled = true;

            var exception = new {context.Exception.Message};
            var result = new JsonResult( exception ) {
                ContentType = "application/json"
            };

            if ( context.Exception is ArgumentNullException || context.Exception is ArgumentException ) {
                result.StatusCode = ( int ) HttpStatusCode.PreconditionFailed;
                context.Result = result;
            } else if ( context.Exception is InvalidOperationException ) {
                result.StatusCode = ( int ) HttpStatusCode.MethodNotAllowed;
                context.Result = result;
            } else if ( context.Exception is NotImplementedException ) {
                result.StatusCode = ( int ) HttpStatusCode.NotImplemented;
                context.Result = result;
            } else if ( context.Exception is NotSupportedException ) {
                result.StatusCode = ( int ) HttpStatusCode.NotImplemented;
                context.Result = result;
            } else if ( context.Exception is ItemNotFoundException ) {
                result.StatusCode = ( int ) HttpStatusCode.NotFound;
                context.Result = result;
            } else if ( context.Exception is UnauthorizedAccessException ) {
                result.StatusCode = ( int ) HttpStatusCode.Unauthorized;
                context.Result = result;
            } else {
                //result.StatusCode = ( int ) HttpStatusCode.InternalServerError;
                //context.Result = result;
                context.ExceptionHandled = false;
            }
        }
    }
}