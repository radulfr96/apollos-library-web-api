using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyLibrary.Application.Book.Queries.GetBookQuery;
using MyLibrary.Application.Common.Behaviour;
using MyLibrary.Persistence.Model;
using MyLibrary.UnitOfWork;
using MyLibrary.UnitOfWork.Contracts;
using MyLibrary.WebApi.Filters;

namespace MyLibrary.WebApi
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

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMediatR(typeof(Startup));

            services.AddScoped<ApiExceptionFilterAttribute>();

            var optionsBuilder = new DbContextOptionsBuilder<MyLibraryContext>();
            services.AddDbContext<MyLibraryContext>(options => options.UseSqlServer(Configuration.GetSection("ConnectionString").Value));
            var context = new MyLibraryContext(optionsBuilder.Options);

            services.AddMediatR(typeof(GetBookQuery).GetTypeInfo().Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddTransient(ser =>
            {
                return new MyLibraryContext(optionsBuilder.Options);
            });

            services.AddTransient(ser =>
            {
                return new MyLibraryContext(optionsBuilder.Options);
            });

            services.AddTransient<IPublisherUnitOfWork>(p =>
            {
                return new PublisherUnitOfWork(context);
            });

            services.AddTransient<IAuthorUnitOfWork>(p =>
            {
                return new AuthorUnitOfWork(context);
            });

            services.AddTransient<IBookUnitOfWork>(p =>
            {
                return new BookUnitOfWork(context);
            });

            services.AddTransient<IGenreUnitOfWork>(p =>
            {
                return new GenreUnitOfWork(context);
            });

            services.AddTransient<IReferenceUnitOfWork>(p =>
            {
                return new ReferenceUnitOfWork(context);
            });

            services.AddControllers();

            var key = Encoding.ASCII.GetBytes(Configuration.GetValue(typeof(string), "TokenKey").ToString());

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(opt =>
                {
                    opt.Authority = "https://localhost:44318";
                    opt.ApiName = "mylibraryapi";
                    opt.ApiSecret = "apisecret";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
