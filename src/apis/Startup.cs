using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace apis
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc()
                .AddControllersAsServices(new[]
                    {
                        typeof(Controllers.ValuesController),
                        typeof(Controllers.ImportController),
                        typeof(Controllers.RecettesController),
                        typeof(Controllers.IngredientsController),
                        typeof(Controllers.CommunityController),
                        typeof(Controllers.AuthenticateController),
                        typeof(Controllers.CommentairesController)
                    });

            services.AddTransient<IUserStore<Models.User>, UserStore<Models.User, IdentityRole<Int32>, Models.NgContext, Int32>>();

            services.AddTransient<IRoleStore<IdentityRole<Int32>>, RoleStore<IdentityRole<Int32>, Models.NgContext, Int32>>();

            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<Models.NgContext>();

            services.AddIdentity<Models.User, IdentityRole<Int32>>(sa =>
            {
                sa.Password.RequireDigit = false;
                sa.Password.RequireUppercase = false;
                sa.Password.RequiredLength = 0;
                sa.Password.RequireNonLetterOrDigit = false;
                sa.Cookies.ApplicationCookie.LoginPath = "/App/Login";
                sa.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(1);
            });

            services.AddInstance<IConfigurationRoot>(Configuration);

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.UseCors(builder => builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().Build());

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseMvc();            
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
