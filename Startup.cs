using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CCT
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            IocContainer.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            // Adds AppDBContext into the ServiceCollection so we can later query it through the provider
            services.AddDbContext<AppDBContext>(options =>
            // Pulls connection string from Configuration
            options.UseSqlServer(IocContainer.Configuration.GetConnectionString("Default")));

            // AddIdendity adds cookie based authentication
            // automatically adds the autheticated user based on their local cookie to the HttpContext.User
            services.AddIdentity<User, IdentityRole>()

                // Tells the AddIdentity where to find the Users and Roles
                .AddEntityFrameworkStores<AppDBContext>()

                //Adds a provider tat genderates unique keys and handles for things like
                // forgot password links, phone number verificaiton codes...
                .AddDefaultTokenProviders();

            // change defaul log-in redirect URL
            services.ConfigureApplicationCookie(options =>
                options.LoginPath = IocContainer.Configuration["LogInURL:Default"]);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {

            // Initializes the provider to be an instance of IServiceProvider
            // (If changed here, please change type in IocContainer)
            IocContainer.provider = serviceProvider;

            // tells it to use the authentication provided in configure
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
