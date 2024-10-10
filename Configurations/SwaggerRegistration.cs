using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using User_API.Filter;

namespace User_API.Configurations
{
    public static class SwaggerRegistration
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "UserAPI", Version = "v1" });
                options.OperationFilter<PreLoginHeaderOperationaFilter>();
                options.OperationFilter<PostLoginHeaderOperationaFilter>();
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //options.IncludeXmlComments(xmlPath);
            });
           
            return services;
        }
    }
}
