using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Todo.API.Swagger;

namespace Todo.API
{
    /// <summary>
    /// Extension methods for IServiceCollection 
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add versioning for API and swagger UI configuration
        /// </summary>
        /// <param name="services"></param>
        public static void AddVersioning(this IServiceCollection services)
        {
            services.AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });
            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
            });
        }

        /// <summary>
        /// Adds SwaggerGen with JWT Authentication
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerGenWithAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Todo API",
                    Description = "API for managing Todo lists and items"
                });
                c.SchemaFilter<ExampleSchemaFilter>();
                // To Enable authorization using Swagger (JWT)  
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] {}
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// Adds JWT Authentication flow
        /// </summary>
        /// <param name="services"></param>
        /// <param name="issuer"></param>
        /// <param name="key"></param>
        public static void AddJwtAuthentication(this IServiceCollection services, string issuer, string key)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
                options.SaveToken = true;
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context => { logger.Error(context.Exception, "OnAuthenticationFailed"); return Task.CompletedTask; },
                    OnForbidden = context => { logger.Error(context?.Result?.Failure, "OnForbidden"); return Task.CompletedTask; },
                    OnTokenValidated = context => { logger.Info("Token Validated"); return Task.CompletedTask; }
                };
            });
        }
    }
}
