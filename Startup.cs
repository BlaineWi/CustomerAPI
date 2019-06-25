using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace customerAPI
{
    /// <summary>
    /// Start Up
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// API Version
        /// </summary>
        public const string ApiVersion = "1.2";

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Property
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(
                 config =>
                 {
                     config.Filters.Add(typeof(GlobalExceptionFilter));
                 }
             );

            string filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "customerAPI.xml");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ApiVersion,
                    new Info
                    {
                        Title = "Customer API",
                        Version = ApiVersion,
                        Contact = new Contact()
                        {
                            Email = "spookdejur@hotmail.com",
                            Name = "Stuart Williams",
                            Url = "https://github.com/BlitzkriegSoftware"
                        },
                        Description = "Customer API DotNet Core + Docker + AKS Demo",
                        License = new License()
                        {
                            Name = "MIT",
                            Url = "https://opensource.org/licenses/MIT"
                        }
                    }
                 );

                c.IncludeXmlComments(filePath);
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IHostingEnvironment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/" + ApiVersion + "/swagger.json", "Customer API " + ApiVersion);
                c.ShowExtensions();
            });
        }
    }
}