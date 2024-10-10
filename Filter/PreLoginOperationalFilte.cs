using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace User_API.Filter
{
    public class PreLoginHeaderOperationaFilter:IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // making custom headers for swagger UI

            var path = context.ApiDescription.RelativePath;
            if (path!=null && path.Contains("PreLogin", StringComparison.OrdinalIgnoreCase))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "ClientId",
                    In = ParameterLocation.Header,
                    Required = true,
                    Schema = new OpenApiSchema { Type = "string" },
                    Description = "Client UserName for pre-login."
                });

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "ClientSecret",
                    In = ParameterLocation.Header,
                    Required = true,
                    Schema = new OpenApiSchema { Type = "string" },
                    Description = "Client Secret for pre-login."
                });
            }
        }
    }
}
