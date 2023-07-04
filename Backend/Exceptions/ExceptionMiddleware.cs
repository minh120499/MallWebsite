using Backend.Model.Response;
using Newtonsoft.Json;

namespace Backend.Exceptions;

using System.Net;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var errorResult = new ErrorResult();
            int statusCode;

            if ((exception is not FormValidationException || exception is not NotFoundException) &&
                exception.InnerException != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
            }

            switch (exception)
            {
                case FormValidationException e:
                    errorResult.Messages = e.ErrorMessages;
                    errorResult.Errors = e.Errors;
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case NotFoundException e:
                    errorResult.Messages = e.Messages;
                    statusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    errorResult.Messages = exception.Message;
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var response = context.Response;
            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                response.StatusCode = statusCode;
                await response.WriteAsync(JsonConvert.SerializeObject(errorResult));
            }
        }
    }
}