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

namespace MyLibrary.Application.UnitTests
{
    public class TestFixture
    {
        public readonly MyLibraryContext context;
        public readonly Configuration configuration;
        public IServiceCollection ServiceCollection { get; private set; }

        public TestFixture()
        {
            ServiceCollection = new ServiceCollection();

            var localConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            configuration = new Configuration();

            var optionsBuilder = new DbContextOptionsBuilder<MyLibraryContext>();
            optionsBuilder.UseSqlServer(localConfig.GetSection("ConnectionString").Value);

            ServiceCollection.AddDbContext<MyLibraryContext>(opt =>
            {
                opt.UseSqlServer(localConfig.GetSection("ConnectionString").Value);
            });
            context = new MyLibraryContext(optionsBuilder.Options);

            ServiceCollection.AddTransient(ser =>
            {
                return new MyLibraryContext(optionsBuilder.Options);
            });

            ServiceCollection.AddTransient(ser =>
            {
                return new MyLibraryContext(optionsBuilder.Options);
            });

            ServiceCollection.AddTransient<IPublisherUnitOfWork>(p => {
                return new PublisherUnitOfWork(context);
            });

            ServiceCollection.AddTransient<IAuthorUnitOfWork>(p => {
                return new AuthorUnitOfWork(context);
            });

            ServiceCollection.AddTransient<IBookUnitOfWork>(p => {
                return new BookUnitOfWork(context);
            });

            ServiceCollection.AddTransient<IGenreUnitOfWork>(p => {
                return new GenreUnitOfWork(context);
            });

            ServiceCollection.AddTransient<IReferenceUnitOfWork>(p => {
                return new ReferenceUnitOfWork(context);
            });

            localConfig.Bind(configuration);

            ServiceCollection.AddHttpContextAccessor();

            ServiceCollection.AddMediatR(typeof(AddAuthorCommand).GetTypeInfo().Assembly);
        }
    }
}
