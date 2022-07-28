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
using Npgsql;

namespace ApollosLibrary.Application.IntegrationTests
{
    public class TestFixture
    {
        public IServiceCollection ServiceCollection { get; private set; }
        private readonly ApollosLibraryContext _context;

        public TestFixture()
        {
            var services = new ServiceCollection();

            var localConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var connectionString = localConfig.GetSection("ConnectionString").Value;
            var conn = connectionString.Replace("{UniqueId}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));

            services.AddDbContext<ApollosLibraryContext>(opt =>
            {
                opt.UseNpgsql(conn, o => o.UseNodaTime());
            });

            services.AddHttpContextAccessor();

            services.AddTransient<IBusinessUnitOfWork>(p =>
            {
                return new BusinessUnitOfWork(p.GetRequiredService<ApollosLibraryContext>());
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

            services.AddTransient<ISeriesUnitOfWork>(p =>
            {
                return new SeriesUnitOfWork(p.GetRequiredService<ApollosLibraryContext>());
            });

            services.AddTransient<IReferenceUnitOfWork>(p =>
            {
                return new ReferenceUnitOfWork(p.GetRequiredService<ApollosLibraryContext>());
            });

            services.AddTransient<ILibraryUnitOfWork>(p =>
            {
                return new LibraryUnitOfWork(p.GetRequiredService<ApollosLibraryContext>());
            });

            services.AddTransient<IOrderUnitOfWork>(p =>
            {
                return new OrderUnitOfWork(p.GetRequiredService<ApollosLibraryContext>());
            });

            services.AddTransient<ISubscriptionUnitOfWork>(p =>
            {
                return new SubscriptionUnitOfWork(p.GetRequiredService<ApollosLibraryContext>());
            });

            services.AddSingleton<IUserService>(p =>
            {
                return new UserService(p.GetRequiredService<IHttpContextAccessor>());
            });

            services.AddSingleton<IModerationUnitOfWork>(p =>
            {
                return new ModerationUnitOfWork(p.GetRequiredService<ApollosLibraryContext>());
            });

            services.AddSingleton<IUserSettingsUnitOfWork>(p =>
            {
                return new UserSettingsUnitOfWork(p.GetRequiredService<ApollosLibraryContext>());
            });

            services.AddHttpContextAccessor();

            services.AddMediatR(typeof(AddAuthorCommand).GetTypeInfo().Assembly);

            var provider = services.BuildServiceProvider();

            _context = provider.GetRequiredService<ApollosLibraryContext>();

            if (!_context.Database.CanConnect())
            {
                _context.Database.Migrate();
            }

            ServiceCollection = services;
        }

        public void DeleteDatabase()
        {
            _context.Database.EnsureCreated();
        }
    }
}
