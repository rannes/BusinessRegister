using System.Reflection;
using BusinessRegister.Api.Filters;
using BusinessRegister.Dal.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BusinessRegister.Api
{
    /// <summary>
    /// Program statup configuration
    /// </summary>
    public class Startup
    {
        private readonly string _apiVersion;

        public Startup(IConfiguration configuration)
        {
            _apiVersion = $"v{Assembly.GetEntryAssembly().GetName().Version}";
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(
                options =>
                {
                    options.Filters.Add(typeof(ExceptionFilter));
                }
            );
            services.Configure<ConnectionString>(Configuration.GetSection("ConnectionStrings"));

            

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors("AllowAll");

            app.UseCookiePolicy();
            app.UseAuthentication();

            //loggerFactory.AddSerilog();

            app.UseMvcWithDefaultRoute();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //// Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger();

            //var virtualDir = "";
            //if (!env.IsDevelopment())
            //{
            //    var webServerUrl = webServerSettings.Value;
            //    virtualDir = webServerUrl.AppVirtualDir;
            //}

            //// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.DocumentTitle = $"Pos API ({_apiVersion}) documentation";
            //    c.SwaggerEndpoint(virtualDir + $"/swagger/{_apiVersion}/swagger.json", $"CompuCash POS API ({_apiVersion})");
            //});
        }
    }
}
