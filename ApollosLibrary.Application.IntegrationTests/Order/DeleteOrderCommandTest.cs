using ApollosLibrary.Application.Common.Enums;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Order.Commands.DeleteOrderCommand;
using ApollosLibrary.Domain;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApollosLibrary.Application.IntegrationTests.Order
{
    [Collection("IntegrationTestCollection")]
    public class DeleteOrderCommandTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IDateTimeService _dateTimeService;

        public DeleteOrderCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            services.AddSingleton(mockDateTimeService.Object);
            _dateTimeService = mockDateTimeService.Object;

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task UpdateOrderCommand()
        {
            var userID = Guid.NewGuid();

            var httpContext = new TestHttpContext
            {
                User = new TestPrincipal(new Claim[]
                {
                    new Claim("userid", userID.ToString()),
                }),
            };

            _contextAccessor.HttpContext = httpContext;

            var business1 = BusinessGenerator.GetGenericBusiness("AU", userID);
            business1.BusinessTypeId = (int)BusinessTypeEnum.Bookshop;
            _context.Business.Add(business1);

            var business2 = BusinessGenerator.GetGenericBusiness("AU", userID);
            business2.BusinessTypeId = (int)BusinessTypeEnum.Bookshop;
            _context.Business.Add(business2);

            var author1 = AuthorGenerator.GetGenericAuthor(userID, "GB");
            _context.Authors.Add(author1);

            var author2 = AuthorGenerator.GetGenericAuthor(userID, "GB");
            _context.Authors.Add(author2);

            var genre1 = GenreGenerator.GetGenre(userID);
            _context.Genres.Add(genre1);

            var genre2 = GenreGenerator.GetGenre(userID);
            _context.Genres.Add(genre2);

            var series1 = SeriesGenerator.GetSeries(userID);
            _context.Series.Add(series1);

            var series2 = SeriesGenerator.GetSeries(userID);
            _context.Series.Add(series2);

            var book1 = BookGenerator.GetGenericPhysicalBook(userID);
            var book2 = BookGenerator.GetGenericPhysicalBook(userID);

            _context.Books.Add(book1);
            _context.Books.Add(book2);

            var order = new Domain.Order()
            {
                Business = business1,
                OrderDate = _dateTimeService.Now,
                UserId = userID,
                OrderItems = new List<OrderItem>()
                {
                    new OrderItem()
                    {
                        BookId = book1.BookId,
                        Price = 10.00m,
                        Quantity = 1,
                    }
                },
            };

            _context.Orders.Add(order);

            _context.SaveChanges();

            var command = new DeleteOrderCommand()
            {
                OrderId = order.OrderId,
            };

            var result = await _mediatr.Send(command);

            var orderDeleted = _context.Orders.FirstOrDefault(o => o.OrderId == command.OrderId);
            orderDeleted.Should().BeNull();

            var orderItems = _context.OrderItems.Where(o => o.OrderId == command.OrderId);
            orderItems.Should().BeEmpty();
        }
    }
}
