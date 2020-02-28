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
using Microsoft.Extensions.FileProviders;
using System.Reflection;

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
        public const string ApiVersion = "1.5";

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
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => {
                // This line must be 1st
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);

                // Console is generically cloud friendly
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            services.AddHealthChecks();

            services.AddHealthChecks().AddCheck<CustomerHealthCheck>("customer_health_check");

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
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/health");

            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "assets","images")),
                RequestPath = new Microsoft.AspNetCore.Http.PathString("/assets/images")
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "assets", "js")),
                RequestPath = new Microsoft.AspNetCore.Http.PathString("/assets/js")
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "assets", "css")),
                RequestPath = new Microsoft.AspNetCore.Http.PathString("/assets/css")
            });

            app.UseMvc();

            var imgPath = env.WebRootPath + "/assets/images/favicon-32x32.png";

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.HeadContent = "<link rel=\"icon\" type=\"image/png\" href=\"" + imgPath + "\" sizes=\"32x32\" />";
                c.InjectStylesheet("/assets/css/Override.css");
                c.InjectJavascript("/assets/js/AddLogo.js");

                c.SwaggerEndpoint("/swagger/" + ApiVersion + "/swagger.json", "Customer API " + ApiVersion);
                c.ShowExtensions();
            });
        }
    }
}