using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenU_BL.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MenU_API
{
    public class Startup
    {
        #region Add Constructor with Configuration using Dependency Injection
        //This constructor need to be added to get an instance of configuration file (appsettings.json)
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        #endregion
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Add the Controllers to the service!
            //Add Controllers and set the Json Serializer to handle loop referencing
            services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions
                        .ReferenceHandler = ReferenceHandler.Preserve);
            services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions
                        .PropertyNameCaseInsensitive = true);
            

            #region Add Session support
            //The following two commands set the Session state to work!
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(180);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                
            });
            #endregion

            #region Add DB Context Support
            string connectionString = this.Configuration.GetConnectionString("MenU");

            services.AddDbContext<MenUContext>(options => options
                                                                .UseSqlServer(connectionString));
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            //Setting the Default Files
            app.UseDefaultFiles(defaultFilesOptions);
            #region Development and Https redirection support
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            #endregion

            #region Static file support
            //To use static files wwwroot (Add wwwroot folder first)
            app.UseStaticFiles();
            #endregion

            app.UseRouting();

            //app.UseAuthorization();

            #region Session support
            //Tells the application to use Session!
            app.UseSession();
            #endregion

            app.UseEndpoints(endpoints =>
            {
                //Map all routings for controllers
                endpoints.MapControllers();
            });

        }
    }
}
