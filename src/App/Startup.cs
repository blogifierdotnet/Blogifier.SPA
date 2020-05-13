using Blogifier.Core;
using Blogifier.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Serilog;
using Serilog.Events;

namespace App
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            Log.Logger = new LoggerConfiguration()
              .Enrich.FromLogContext()
              .WriteTo.RollingFile("Logs/{Date}.txt", LogEventLevel.Warning)
              .CreateLogger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBlogDatabase(Configuration);
            services.AddBlogSecurity();
            services.AddBlogLocalization();

            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            services.AddFeatureManagement();

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                );
            });

            services.AddControllersWithViews()
                .AddViewLocalization();

            services.AddRazorPages(
                options => options.Conventions.AuthorizeFolder("/Admin")
            );

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "wwwroot/themes/_active";
            });

            services.AddBlogServices();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRequestLocalization();        

            AppSettings.WebRootPath = Environment.WebRootPath;
            AppSettings.ContentRootPath = Environment.ContentRootPath;

            app.UseRouting();
            app.UseCors("AllowOrigin");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Blog}/{action=Index}/{id?}"
                );
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
            });
        }
    }
}