using System;
using System.IO;
using System.Reflection;
using CustomerAPI3.Libs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace CustomerAPI3
{
    /// <summary>
    /// Start Up Builder
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Configuration Property
        /// </summary>
        public IConfiguration Configuration { get; }

        private const string CommonVersion = "common";

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => {
                // This line must be 1st
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);

                // Console is generically cloud friendly
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            services.AddControllers();

            services.AddHealthChecks();

            services.AddCors(cors =>
            {
                cors.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin();
                });
            });

            services.AddMvc(
                config =>
                {
                    config.Filters.Add(typeof(GlobalExceptionFilter));
                }
            );

            services.AddSwaggerGen(swag =>
            {

                swag.SwaggerDoc(Program.ProgramMetadata.MajorVersion,
                    new OpenApiInfo()
                    {
                        Contact = new OpenApiContact()
                        {
                            Email = "spookdejur@hotmail.com",
                            Name = "Stuart Williams",
                            Url = new Uri("https://github.com/blitzkriegsoftware/customerapi")
                        },
                        Description = Program.ProgramMetadata.Description,
                        License = new OpenApiLicense()
                        {
                            Name = "MIT",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        },
                        Title = Program.ProgramMetadata.Description,
                        Version = Program.ProgramMetadata.SemanticVersion
                    });

                swag.SwaggerDoc(CommonVersion, new OpenApiInfo { Title = "Common Operations", Version = CommonVersion });

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (System.IO.File.Exists(xmlPath)) swag.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IWebHostEnvironment</param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (app == null) return;
            if (env == null) return;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "assets", "images")),
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

            var imgPath = env.WebRootPath + "/assets/images/favicon-32x32.png";

            app.UseSwagger();

            app.UseSwaggerUI(ui =>
            {
                ui.HeadContent = "<link rel=\"icon\" type=\"image/png\" href=\"" + imgPath + "\" sizes=\"32x32\" />";
                ui.InjectStylesheet("/assets/css/Override.css");
                ui.InjectJavascript("/assets/js/AddLogo.js");

                ui.SwaggerEndpoint($"/swagger/{Program.ProgramMetadata.MajorVersion}/swagger.json", $"{Program.ProgramMetadata.Description} {Program.ProgramMetadata.MajorVersion}");
                ui.SwaggerEndpoint($"/swagger/{CommonVersion}/swagger.json", $"Common {Program.ProgramMetadata.MajorVersion}");

                ui.ShowExtensions();
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
