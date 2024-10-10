using Newtonsoft.Json;
using System.Net;
using Serilog;
using Microsoft.IdentityModel.Tokens;

namespace User_API.ExceptionHandlerMiddleware
{
    public class ExceptionHandler(RequestDelegate next)
    {
        private readonly RequestDelegate next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context); // sending or passing the control to the next middleware
            }
            catch (CustomException ex)
            {
                LogError(ex, context.Response.StatusCode);
                await HandleCustomException(context, ex);
            }
            //next three exceptions are a part of the ValidateToken class in USerVerificationFilter.
            catch (SecurityTokenExpiredException ex)
            {
                LogError(ex, context.Response.StatusCode);
                await HandleGenericExceptionAsync(context, ex, "Token Expired");
            }
            catch (SecurityTokenException ex)
            {
                LogError(ex, context.Response.StatusCode);
                await HandleGenericExceptionAsync(context, ex, "Invalid token");
            }
            catch (SecurityTokenMalformedException ex)
            {
                LogError(ex, context.Response.StatusCode);
                await HandleGenericExceptionAsync(context, ex, "Token format wrong");
            }
            catch (Exception ex)
            {
                LogError(ex, (int)HttpStatusCode.InternalServerError);
                await HandleGenericExceptionAsync(context, ex, null);
            }
        }
        private static void LogError(Exception ex, int statusCode)
        {
            var exceptionType = ex.GetType().Name;
            var message = ex.Message;
            var stackTrace = ex.StackTrace;

            // Log the error details with Serilog
            Log.Error("Exception caught: {ExceptionType} | StatusCode: {StatusCode} | Message: {Message} | StackTrace: {StackTrace}",
                exceptionType, statusCode, message, stackTrace);
        }

        private static Task HandleCustomException(HttpContext context, CustomException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)ex.ErrorCode;
            var response = new CommonResponse(ex.ErrorCode,ex.Data);
            //convert response to json (displayed on swagger)
            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

        private static Task HandleGenericExceptionAsync(HttpContext context, Exception ex, string? message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var error_ = ex.Message;
            if (message != null) error_ = message;
            var response = new
            {
                error = error_,
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }

    public class CustomException: Exception
    {
        public ErrorCode ErrorCode { get; set; }
        public object? Data { get; set; }
        public CustomException(ErrorCode errorCode, object? data = null)
        {
            ErrorCode = errorCode;
            Data = data;
        }
    }
}




