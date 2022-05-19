using ApollosLibrary.Application.Common.Enums;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Order;
using ApollosLibrary.Application.Order.Commands.AddOrderCommand;
using ApollosLibrary.Application.Order.Commands.UpdateOrderCommand;
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
    public class UpdateOrderCommandTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IDateTimeService _dateTimeService;

        public UpdateOrderCommandTest(TestFixture fixture) : base(fixture)
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
                        Book = book1,
                        Price = 10.00m,
                        Quantity = 1,
                    }
                },
            };

            _context.Orders.Add(order);

            _context.SaveChanges();

            var command = new UpdateOrderCommand()
            {
                OrderId = order.OrderId,
                BusinessId = business2.BusinessId,
                OrderDate = _dateTimeService.Now.AddDays(2),
                OrderItems = new List<OrderItemDTO>()
                {
                    new OrderItemDTO()
                    {
                        BookId = book2.BookId,
                        UnitPrice = 15.00m,
                        Quantity = 2,
                    },
                },
            };

            var result = await _mediatr.Send(command);

            var orderUpdated = _context.Orders
                                .Include(o => o.OrderItems)
                                .FirstOrDefault(o => o.OrderId == command.OrderId);

            orderUpdated.Should().NotBeNull();
            orderUpdated.Should().BeEquivalentTo(new Domain.Order()
            {
                BusinessId = command.BusinessId,
                OrderDate = command.OrderDate,
                UserId = userID,
                OrderId = order.OrderId,
            }, opt => opt.Excluding(f => f.Business).Excluding(f => f.OrderItems));

            orderUpdated.OrderItems.First().Should().BeEquivalentTo(new OrderItem()
            {
                BookId = book2.BookId,
                OrderId = command.OrderId,
                Price = command.OrderItems.First().UnitPrice,
                Quantity = command.OrderItems.First().Quantity,
            }, opt => opt.Excluding(f => f.OrderItemId).Excluding(f => f.Order).Excluding(f => f.Book));
        }
    }
}
