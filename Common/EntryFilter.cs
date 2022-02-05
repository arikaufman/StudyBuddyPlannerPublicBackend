using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using plannerBackEnd.Common.DomainObjects;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Extensions;
using plannerBackEnd.Admin.Domain;
using plannerBackEnd.Admin.Domain.DomainObjects;

namespace plannerBackEnd.Common
{
    public class RateLimitStatus
	{
		public DateTime LastCheck { get; set; } = DateTime.MinValue;
		public double Allowance { get; set; } = 0;
	}

	public class EntryFilter : Attribute, IActionFilter
	{

		private static MemoryCache RateLimitCache { get; } = new MemoryCache(new MemoryCacheOptions());

		public void OnActionExecuting(ActionExecutingContext context)
		{
			RequestContext requestContext = setRequestContext(context);
			//checkRateLimit(requestContext);
			//logRouteAccess(context);
		}

		// ---------------------------------------------------------------------------------------------------------------
		private RequestContext setRequestContext(ActionExecutingContext context)
		{
			ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(context.HttpContext.User.Identity);

			var requestContext = context.HttpContext.RequestServices.GetService<RequestContext>();

			requestContext.RequestStartTime = DateTime.Now;
            int UserId = 0;
            Int32.TryParse(claimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault(), out UserId);
            requestContext.UserId = UserId;
            requestContext.ClientIp = context.HttpContext.Connection.RemoteIpAddress?.ToString();

            return requestContext;
		}

		/*// ---------------------------------------------------------------------------------------------------------------
		private static void logRouteAccess(ActionExecutingContext context)
		{
			//var logger = context.HttpContext.RequestServices.GetService<ILogger>();
			//logger.LogAccess(context.HttpContext.Request.Method, context.HttpContext.Request.Path + " ", 
			//  context.HttpContext.Request.Path + " " + JsonConvert.SerializeObject(context.ActionArguments),  ;
		}*/

		/*// ------------------------------------------------------------------------------------------------------------------------
		private void checkRateLimit(RequestContext requestContext)
		{
			double maxRequests = 5; // Requests
			double per = 60; // Seconds

            var memoryCacheKey = requestContext.UserId;

            RateLimitCache.TryGetValue(memoryCacheKey, out RateLimitStatus rateLimitStatus);
			if (rateLimitStatus == null)
			{
				RateLimitStatus newRateLimitStatus = new RateLimitStatus
				{
					LastCheck = DateTime.Now,
					Allowance = maxRequests - 1.0
				};
				RateLimitCache.Set(memoryCacheKey, newRateLimitStatus);
			}
			else
			{
				DateTime now = DateTime.Now;
				TimeSpan timePassed = now - rateLimitStatus.LastCheck;
				rateLimitStatus.LastCheck = now;

				rateLimitStatus.Allowance += timePassed.TotalSeconds * (maxRequests / per);
				if (rateLimitStatus.Allowance > maxRequests)
					rateLimitStatus.Allowance = maxRequests;

				if (rateLimitStatus.Allowance < 1.0)
					throw new Exception($"Too many requests. Requests are limited to {maxRequests} every {per} seconds.");

				rateLimitStatus.Allowance -= 1.0;

				RateLimitCache.Set(memoryCacheKey, rateLimitStatus);
			}
		}
		*/
		// ---------------------------------------------------------------------------------------------------------------
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //var adminService = context.HttpContext.RequestServices.GetService<IAdminService>();
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(context.HttpContext.User.Identity);

            int UserId = 0;
            Int32.TryParse(claimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault(), out UserId);
			DateTime endTime = DateTime.Now;
            var requestContext = context.HttpContext.RequestServices.GetService<RequestContext>();
            TimeSpan duration = endTime - requestContext.RequestStartTime;
            /*adminService.CreateAccessLogEntry(new AccessLog()
            {
                Duration = duration.Seconds,
                HttpMethod = context.HttpContext.Request.Method,
				HttpRequest = context.HttpContext.Request.Path,
				Url = context.HttpContext.Request.GetDisplayUrl(),
				User = UserId

            });*/
        }
	}
}
