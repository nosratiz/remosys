using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Remosys.Common.Swagger
{
    public static class Extensions
    {

        public static IServiceCollection AddSwaggerDocs(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SwaggerSettings>(configuration.GetSection("swaggerSettings"));

            var swaggerSettings = new SwaggerSettings();
            configuration.Bind(nameof(SwaggerSettings), swaggerSettings);

            services.AddSingleton(swaggerSettings);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(swaggerSettings.Version, new OpenApiInfo
                {
                    Title = swaggerSettings.Title,
                    Version = swaggerSettings.Version,
                    Description = swaggerSettings.Description
                });

                //Set the comments path for the Swagger JSON and UI.

               var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));

                if (swaggerSettings.IncludeSecurity)
                {
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                    });
                }

            });

            return services;
        }
        public static IApplicationBuilder UseSwaggerDocs(this IApplicationBuilder builder)
        {
            builder.UseSwagger();
            builder.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Remosys Api "); });

            return builder;
        }
    }


}