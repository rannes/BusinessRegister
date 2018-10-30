using System;
using System.IO;
using System.Reflection;
using BusinessRegister.Api.Filters;
using BusinessRegister.Api.Services;
using BusinessRegister.Dal.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace BusinessRegister.Api
{
    /// <summary>
    /// Program statup configuration
    /// </summary>
    public class Startup
    {
        private readonly string _apiVersion;

        /// <summary>
        /// <see cref="IConfiguration"/> property of Startup class
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <inheritdoc />
        public Startup(IConfiguration configuration)
        {
            _apiVersion = $"v{Assembly.GetEntryAssembly().GetName().Version}";
            Configuration = configuration;
        }

        /// <summary>
        /// Called at runtime.
        /// Used to add services to the container
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(
                options =>
                {
                    options.Filters.Add(typeof(ExceptionFilter));
                }
            );

            services.Configure<ConnectionString>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<WebServerSettings>(Configuration.GetSection("WebServerSettings"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    _apiVersion,
                    new Info
                    {
                        Title = $"Business Register API ({_apiVersion})",
                        Version = _apiVersion
                    });

                var basePath = AppContext.BaseDirectory;
                c.IncludeXmlComments(Path.Combine(basePath, "BusinessRegister.Api.xml"));
                c.IncludeXmlComments(Path.Combine(basePath, "BusinessRegister.Dal.xml"));
                c.DescribeAllEnumsAsStrings();
            });

            services.AddHostedService<RegistryDataUpdaterService>();
        }

        /// <summary>
        /// This method gets called by the runtime.
        /// Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/></param>
        /// <param name="env"><see cref="IHostingEnvironment"/></param>
        /// <param name="loggerFactory"><see cref="ILoggerFactory"/></param>
        /// <param name="webServerSettings"><see cref="IOptions{TOptions}"/></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<WebServerSettings> webServerSettings)
        {
            app.UseMvcWithDefaultRoute();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            //If in development mode then ignore VirtualDir from AppSettings
            var virtualDir = "";
            if (!env.IsDevelopment())
            {
                virtualDir = webServerSettings.Value.VirtualDir;
            }

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = $"Business Register API ({_apiVersion}) documentation";
                c.SwaggerEndpoint(virtualDir + $"/swagger/{_apiVersion}/swagger.json", $"Business Register API ({_apiVersion})");
            });
        }
    }
}
