using System;

namespace plannerBackEnd.Common.Swagger
{
    public class SwaggerRouteDescriptionAttribute : Attribute
    {
        public string Description { get; set; } = "";
        public string Detail { get; set; } = "";
    }
}
