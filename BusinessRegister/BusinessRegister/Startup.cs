using System.Reflection;
using BusinessRegister.Api.Filters;
using BusinessRegister.Dal.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
