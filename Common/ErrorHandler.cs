using System;
using System.IO;
using System.Reflection;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using plannerBackEnd.Admin.Domain;
using plannerBackEnd.Admin.Domain.DomainObjects;
using plannerBackEnd.Common.DomainObjects;

namespace plannerBackEnd.Common
{
	public class ErrorHandler
	{
		private readonly RequestDelegate next;

		// ------------------------------------------------------------------------------------------
		public ErrorHandler(RequestDelegate next)
		{
			this.next = next;
		}
		// ------------------------------------------------------------------------------------------
		public async Task Invoke(HttpContext context /* other scoped dependencies */)
		{
			try
			{
				// Call the next delegate/middleware in the pipeline
				await next(context);
			}
            catch (Exception ex)
            {
                await handleExceptionAsync(context, (Exception)ex);
            }
            return;
		}
		// ------------------------------------------------------------------------------------------
		private Task handleExceptionAsync(HttpContext context, Exception exception)
        {
            var adminService = context.RequestServices.GetService<IAdminService>();
            var requestContext = context.RequestServices.GetService<RequestContext>();

            int user = requestContext.UserId;
            adminService.CreateErrorLogEntry(new ErrorLog
            {
                Comments = exception.Message,
                CallStack = exception.StackTrace,
                User = user
            });

			if (exception is AuthenticationException)
            {
                context.Response.StatusCode = 401;
            }
            else if (exception is InvalidDataException)
            {
                context.Response.StatusCode = 415;
            }
            else if (exception is SystemException)
            {
                context.Response.StatusCode = 500;
            }
			context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(exception.Message);

			return context.Response.WriteAsync(result);

			}
    }
}
