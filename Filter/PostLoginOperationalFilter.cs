using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace User_API.Filter
{
    public class PostLoginHeaderOperationaFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // making custom headers for swagger UI wherever it sees postlogin

            var path = context.ApiDescription.RelativePath;
            
            if (path !=null && path.Contains("PostLogin", StringComparison.OrdinalIgnoreCase))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "token",
                    In = ParameterLocation.Header,
                    Required = true,
                    Schema = new OpenApiSchema { Type = "string" },
                    Description = "Bearer Token"
                });
            }
        }
    }
}

