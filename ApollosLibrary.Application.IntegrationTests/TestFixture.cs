using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ApollosLibrary.Application.Author.Commands.AddAuthorCommand;
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
using ApollosLibrary.Domain;

namespace ApollosLibrary.Application.IntegrationTests
{
    public class TestFixture
    {
        public IServiceCollection ServiceCollection { get; private set; }
        private readonly ApollosLibraryContext _context;
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

            var optionsBuilder = new DbContextOptionsBuilder<ApollosLibraryContext>();
            optionsBuilder.UseSqlServer(localConfig.GetSection("ConnectionString").Value);

            services.AddHttpContextAccessor();
            services.AddDbContext<ApollosLibraryContext>(opt =>
            {
                opt.UseSqlServer(localConfig.GetSection("ConnectionString").Value);
            });

            services.AddTransient<IPublisherUnitOfWork>(p =>
            {
                return new PublisherUnitOfWork(p.GetRequiredService<ApollosLibraryContext>());
            });

            services.AddTransient<IAuthorUnitOfWork>(p =>
            {
                return new AuthorUnitOfWork(p.GetRequiredService<ApollosLibraryContext>());
            });

            services.AddTransient<IBookUnitOfWork>(p =>
            {
                return new BookUnitOfWork(p.GetRequiredService<ApollosLibraryContext>());
            });

            services.AddTransient<IGenreUnitOfWork>(p =>
            {
                return new GenreUnitOfWork(p.GetRequiredService<ApollosLibraryContext>());
            });

            services.AddTransient<IReferenceUnitOfWork>(p =>
            {
                return new ReferenceUnitOfWork(p.GetRequiredService<ApollosLibraryContext>());
            });

            services.AddSingleton<IUserService>(p =>
            {
                return new UserService(p.GetRequiredService<IHttpContextAccessor>());
            });

            localConfig.Bind(_configuration);

            services.AddHttpContextAccessor();

            services.AddMediatR(typeof(AddAuthorCommand).GetTypeInfo().Assembly);

            var provider = services.BuildServiceProvider();

            _context = provider.GetRequiredService<ApollosLibraryContext>();

            if (!_context.Database.CanConnect())
            {
                _context.Database.Migrate();
            }

            _checkpoint = new Checkpoint
            {
                TablesToIgnore = new Table[] { "Countries", "FictionTypes", "FormTypes", "PublicationFormats" },
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
            _checkpoint.Reset(_context.Database.GetConnectionString())
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }
    }
}
