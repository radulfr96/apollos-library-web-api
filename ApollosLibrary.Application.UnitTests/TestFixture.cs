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

            ServiceCollection.AddDbContext<ApollosLibraryContext>(opt =>
            {
                opt.UseInMemoryDatabase("ApollosLibrayUnitTestDB");
            });

            localConfig.Bind(configuration);

            ServiceCollection.AddDbContext<ApollosLibraryContext>();

            ServiceCollection.AddHttpContextAccessor();

            ServiceCollection.AddMediatR(typeof(AddAuthorCommand).GetTypeInfo().Assembly);
        }
    }
}
