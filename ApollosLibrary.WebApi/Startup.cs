using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
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
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ApollosLibrary.Application.Book.Queries.GetBookQuery;
using ApollosLibrary.Application.Common.Behaviour;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Infrastructure.Services;

using ApollosLibrary.UnitOfWork;
using ApollosLibrary.UnitOfWork.Contracts;
using ApollosLibrary.WebApi.Filters;
using NLog;
using ApollosLibrary.Domain;

namespace ApollosLibrary.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILogger _logger = LogManager.GetCurrentClassLogger();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddHttpContextAccessor();
            services.AddMediatR(typeof(Startup));

            services.AddScoped<ApiExceptionFilterAttribute>();

            services.AddDbContext<ApollosLibraryContext>(options => options.UseSqlServer(Configuration.GetSection("ConnectionString").Value));

            services.AddMediatR(typeof(GetBookQuery).GetTypeInfo().Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddTransient<IBusinessUnitOfWork, BusinessUnitOfWork>();
            services.AddTransient<IAuthorUnitOfWork, AuthorUnitOfWork>();
            services.AddTransient<IBookUnitOfWork, BookUnitOfWork>();
            services.AddTransient<IGenreUnitOfWork,GenreUnitOfWork>();
            services.AddTransient<IReferenceUnitOfWork, ReferenceUnitOfWork>();
            services.AddTransient<ISeriesUnitOfWork, SeriesUnitOfWork>();
            services.AddTransient<IUserService,UserService>();
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddScoped<DbContext, ApollosLibraryContext>();

            services.AddSwaggerGen(c =>
            {
                // configure SwaggerDoc and others

                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });
            });

            services.AddControllers();


            var key = Encoding.ASCII.GetBytes(Configuration.GetValue(typeof(string), "TokenKey").ToString());

            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:5001";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    NameClaimType = ClaimTypes.Name,
                };

                options.SaveToken = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DbContext dbContext)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            dbContext.Database.Migrate();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
