using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using plannerBackEnd.Common.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Swagger
{
    public class ApplySwaggerDescriptionAttribute : IDocumentFilter
	{

        public ApplySwaggerDescriptionAttribute()
		{
        }

		// ----------------------------------------------------------------------------------------------------------------------
		public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
		{
            addRouteSummaries(swaggerDoc, context);

		}

		
		// ----------------------------------------------------------------------------------------------------------------------
		private void addRouteSummaries(OpenApiDocument swaggerDoc, DocumentFilterContext context)
		{
			var paths = new Dictionary<string, OpenApiPathItem>(swaggerDoc.Paths);

			foreach (var path in swaggerDoc.Paths)
			{
				foreach (var operation in path.Value.Operations)
				{
					try
					{
						var actionDescriptor = (ControllerActionDescriptor)context.ApiDescriptions.FirstOrDefault(
							x => path.Key == "/" + x.RelativePath && operation.Key.ToString().ToLower() == x.HttpMethod.ToLower()).ActionDescriptor;


						SwaggerRouteDescriptionAttribute attribute =
							actionDescriptor.MethodInfo.GetCustomAttributes<SwaggerRouteDescriptionAttribute>().FirstOrDefault();

                        if (attribute != null)
                        {
                            operation.Value.Summary = attribute.Description;
                            operation.Value.Description = attribute.Detail;
                            operation.Value.OperationId = actionDescriptor.DisplayName;
                        }
                    }
					catch
					{

					}
				}
			}
		}
    }
}