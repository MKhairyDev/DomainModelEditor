using System.Reflection;
using DomainModelEditor.Api.Rest.Controllers;
using DomainModelEditor.Api.Rest.Services;
using DomainModelEditor.Data;
using DomainModelEditor.Data.Abstractions;
using DomainModelEditor.Data.SqlServer;
using DomainModelEditor.Data.SqlServer.Repositories;
using DomainModelEditor.Data.SqlServer.Services;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DomainModelEditor.Api.Rest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*
             •	We could have supported Microsoft Response Caching middleware store but it is only good with simple cases
                but with complex cases other options could be better.
             •	The middleware determines when responses are cacheable, stores responses, and serves responses from cache.
            
              // Adding it to the ServiceCollection
              services.AddResponseCaching();
              //Use cache profiles to apply the same rules to different resources.
            services.AddControllers(action =>
            {
                action.CacheProfiles.Add("30SecondsProfile", new CacheProfile() { Duration = 30 });
            });

            */

            /*
             
            •	Asp.net core middleware that add a HTTP cache header to responses, like Cache-Control, Expires, ETag and Last-Modified.
            •	Implements cache expiration and validation model.
            •	We are setting below the global Cache header configuration, but it could be overridden by Controller or Action level.

            */

            services.AddHttpCacheHeaders(expirationModelOptions =>
                {
                    //Enable Cache Expiration Model.
                    expirationModelOptions.MaxAge = 60;
                    expirationModelOptions.CacheLocation = CacheLocation.Private;
                },
                validationModelOptions =>
                {
                    //Enable Cache Validation Model, it tells the cache that if the response becomes stall, re-validation must happen.
                    validationModelOptions.MustRevalidate = true;
                });

            services.AddControllers();
            // register PropertyCheckerService
            services.AddTransient<IPropertyCheckerService, PropertyCheckerService>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //Support Versioning
            services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ReportApiVersions = true;
                //in this way we could have one or more methods for figuring out what version is!
                opt.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("X-Version"),
                    new QueryStringApiVersionReader("ver", "Version"));
                //By this way we centralize our versioning information which is better than attribute versioning in the Controller
                opt.Conventions.Controller<EntitiesController>().HasApiVersion(new ApiVersion(1, 0));
            });

            services.AddDbContext<EntityContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("EntityContext"));
            });

            //Register all Repositories & services
            // register PropertyCheckerService

            services.AddScoped<IEntityRepository, EntityRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IPaginationService<>), typeof(PaginationService<>));
            services.AddScoped(typeof(ILinksCreation), typeof(LinksCreationForEntity));
            services.AddTransient<IPropertyCheckerService, PropertyCheckerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                // Adding middleware to catch exception instead of the redundant "try catch" in Controllers.
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            // Adds Response Caching middleware to the request processing pipeline .
            //app.UseResponseCaching();

            //Middleware that add a HTTP cache header to responses, like Cache - Control, Expires, ETag and Last - Modified.

            app.UseHttpCacheHeaders();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}