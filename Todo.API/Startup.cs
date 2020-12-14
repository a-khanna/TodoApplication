using System;
using System.Linq;
using AutoMapper;
using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using Todo.API.GraphQL;
using Todo.API.Middlewares;
using Todo.Core;
using Todo.Core.Logging;
using Todo.Data;

namespace Todo.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            NLog.LogManager.Configuration = new NLogLoggingConfiguration(Configuration.GetSection(Constants.NLogKey));
            services.AddSingleton(typeof(ILogger<>), typeof(NLogLogger<>));

            var connectionString = Configuration.GetConnectionString(Constants.ConnectionStringKey);
            services.RegisterDataDependencies(connectionString);
            services.RegisterLogicDependencies();
            services.AddHttpContextAccessor();

            services.RegisterMiddlewares();

            services.AddVersioning();

            var jwtKey = Configuration.GetValue<string>(Constants.AppSettingsJwtKey);
            var jwtIssuer = Configuration.GetValue<string>(Constants.AppSettingsJwtIssuer);
            services.AddJwtAuthentication(jwtIssuer, jwtKey);
            services.AddSwaggerGenWithAuth();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddGraphQL(s => SchemaBuilder.New()
                    .AddServices(s)
                    .AddQueryType<Query>()
                    .AddMutationType<Mutation>()
                    .Create());

            services.AddControllers(options =>
            {
                options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
                options.RespectBrowserAcceptHeader = true;
            })
            .AddXmlSerializerFormatters()
            .AddXmlDataContractSerializerFormatters()
            .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TodoDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseExceptionHandling();
            app.UseCorrelationId();
            app.UseRequestLogging();
            app.UseContentLocation();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseGraphQLJWTMiddleware();
            app.UseGraphQL().UsePlayground();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Seed database only if seeding is enabled in appsettings (default = true)
            if (Configuration.GetValue<bool>(Constants.SeedKey))
                dbContext.Seed();
        }

        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }
    }
}
