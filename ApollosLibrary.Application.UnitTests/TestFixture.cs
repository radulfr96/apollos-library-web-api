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
using ApollosLibrary.Domain;

namespace ApollosLibrary.Application.UnitTests
{
    public class TestFixture
    {
        public readonly ApollosLibraryContext context;
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

            var optionsBuilder = new DbContextOptionsBuilder<ApollosLibraryContext>();
            optionsBuilder.UseSqlServer(localConfig.GetSection("ConnectionString").Value);

            ServiceCollection.AddDbContext<ApollosLibraryContext>(opt =>
            {
                opt.UseSqlServer(localConfig.GetSection("ConnectionString").Value);
            });
            context = new ApollosLibraryContext(optionsBuilder.Options);

            ServiceCollection.AddTransient(ser =>
            {
                return new ApollosLibraryContext(optionsBuilder.Options);
            });

            localConfig.Bind(configuration);

            ServiceCollection.AddDbContext<ApollosLibraryContext>();

            ServiceCollection.AddHttpContextAccessor();

            ServiceCollection.AddMediatR(typeof(AddAuthorCommand).GetTypeInfo().Assembly);
        }
    }
}
