using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ApollosLibrary.Application.Author.Commands.AddAuthorCommand;
using ApollosLibrary.Persistence.Model;
using Microsoft.Extensions.Configuration;
using ApollosLibrary.Application.Common;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ApollosLibrary.UnitOfWork.Contracts;
using ApollosLibrary.UnitOfWork;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Infrastructure.Services;
using Respawn;
using Microsoft.AspNetCore.Http;
using Respawn.Graph;

namespace ApollosLibrary.Application.IntegrationTests
{
    public class TestFixture
    {
        public IServiceCollection ServiceCollection { get; private set; }
        private readonly ApollosLibraryContextOld _context;
        private readonly Configuration _configuration;
        private readonly Checkpoint _checkpoint;

        public TestFixture()
        {
            var services = new ServiceCollection();

            var localConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            _configuration = new Configuration();

            var optionsBuilder = new DbContextOptionsBuilder<ApollosLibraryContextOld>();
            optionsBuilder.UseSqlServer(localConfig.GetSection("ConnectionString").Value);

            services.AddHttpContextAccessor();
            services.AddDbContext<ApollosLibraryContextOld>(opt =>
            {
                opt.UseSqlServer(localConfig.GetSection("ConnectionString").Value);
            });
            _context = new ApollosLibraryContextOld(optionsBuilder.Options);

            services.AddTransient<IPublisherUnitOfWork>(p => {
                return new PublisherUnitOfWork(p.GetRequiredService<ApollosLibraryContextOld>());
            });

            services.AddTransient<IAuthorUnitOfWork>(p => {
                return new AuthorUnitOfWork(p.GetRequiredService<ApollosLibraryContextOld>());
            });

            services.AddTransient<IBookUnitOfWork>(p => {
                return new BookUnitOfWork(p.GetRequiredService<ApollosLibraryContextOld>());
            });

            services.AddTransient<IGenreUnitOfWork>(p => {
                return new GenreUnitOfWork(p.GetRequiredService<ApollosLibraryContextOld>());
            });

            services.AddTransient<IReferenceUnitOfWork>(p => {
                return new ReferenceUnitOfWork(p.GetRequiredService<ApollosLibraryContextOld>());
            });

            services.AddTransient<IUserUnitOfWork>(p => {
                return new UserUnitOfWork(p.GetRequiredService<ApollosLibraryContextOld>());
            });

            services.AddSingleton<IUserService>(p =>
            {
                return new UserService(p.GetRequiredService<IHttpContextAccessor>());
            });


            localConfig.Bind(_configuration);

            services.AddHttpContextAccessor();

            services.AddMediatR(typeof(AddAuthorCommand).GetTypeInfo().Assembly);

            _checkpoint = new Checkpoint
            {
                SchemasToInclude = new string[] { "Author", "Book", "Genre", "Publisher" },
                TablesToInclude = new Table[] { "Author", "Book", "Genre", "BookAuthor", "BookGenre", "Publisher" },
            };

            ServiceCollection = services;
        }

        public Configuration Configuration
        { 
            get
            {
                return _configuration;
            }
        }

        public void ResetCheckpoint()
        {
            _checkpoint.Reset(_configuration.ConnectionString).Wait();
        }
    }
}
