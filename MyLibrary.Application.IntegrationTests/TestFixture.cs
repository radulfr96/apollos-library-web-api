using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MyLibrary.Application.Author.Commands.AddAuthorCommand;
using MyLibrary.Persistence.Model;
using Microsoft.Extensions.Configuration;
using MyLibrary.Application.Common;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using MyLibrary.UnitOfWork.Contracts;
using MyLibrary.UnitOfWork;
using MyLibrary.Application.Interfaces;
using MyLibrary.Infrastructure.Services;
using Respawn;

namespace MyLibrary.Application.IntegrationTests
{
    public class TestFixture
    {
        public IServiceCollection ServiceCollection { get; private set; }
        private readonly MyLibraryContext _context;
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

            var optionsBuilder = new DbContextOptionsBuilder<MyLibraryContext>();
            optionsBuilder.UseSqlServer(localConfig.GetSection("ConnectionString").Value);

            services.AddDbContext<MyLibraryContext>(opt =>
            {
                opt.UseSqlServer(localConfig.GetSection("ConnectionString").Value);
            });
            _context = new MyLibraryContext(optionsBuilder.Options);

            services.AddTransient<IPublisherUnitOfWork>(p => {
                return new PublisherUnitOfWork(p.GetRequiredService<MyLibraryContext>());
            });

            services.AddTransient<IAuthorUnitOfWork>(p => {
                return new AuthorUnitOfWork(p.GetRequiredService<MyLibraryContext>());
            });

            services.AddTransient<IBookUnitOfWork>(p => {
                return new BookUnitOfWork(p.GetRequiredService<MyLibraryContext>());
            });

            services.AddTransient<IGenreUnitOfWork>(p => {
                return new GenreUnitOfWork(p.GetRequiredService<MyLibraryContext>());
            });

            services.AddTransient<IReferenceUnitOfWork>(p => {
                return new ReferenceUnitOfWork(p.GetRequiredService<MyLibraryContext>());
            });

            services.AddSingleton<IUserService>(p =>
            {
                return new UserService();
            });

            localConfig.Bind(_configuration);

            services.AddHttpContextAccessor();

            services.AddMediatR(typeof(AddAuthorCommand).GetTypeInfo().Assembly);

            _checkpoint = new Checkpoint();
            _checkpoint.SchemasToInclude = new string[] { "Author" };
            _checkpoint.TablesToInclude = new string[] { "Author" };

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
