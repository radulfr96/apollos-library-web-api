using ApollosLibrary.Application.Common.Enums;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Order;
using ApollosLibrary.Application.Order.Commands.AddOrderCommand;
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
    public class AddOrderCommandTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IDateTimeService _dateTimeService;

        public AddOrderCommandTest(TestFixture fixture) : base(fixture)
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
        public async Task AddOrderCommand()
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

            var business = BusinessGenerator.GetGenericBusiness("AU", userID);
            business.BusinessTypeId = (int)BusinessTypeEnum.Bookshop;
            _context.Business.Add(business);

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

            var book = BookGenerator.GetGenericPhysicalBook(userID);

            _context.Books.Add(book);

            _context.SaveChanges();

            var command = new AddOrderCommand()
            {
                BusinessId = business.BusinessId,
                OrderDate = _dateTimeService.Now,
                OrderItems = new List<OrderItemDTO>()
                {
                    new OrderItemDTO()
                    {
                        BookId = book.BookId,
                        Price = 10.00m,
                        Quantity = 1,
                    },
                },
            };

            var result = await _mediatr.Send(command);

            var order = _context.Orders
                                .Include(o => o.OrderItems)
                                .FirstOrDefault(o => o.OrderId == result.OrderId);

            order.Should().NotBeNull();
            order.Should().BeEquivalentTo(new Domain.Order()
            {
                BusinessId = order.BusinessId,
                OrderDate = order.OrderDate,
                UserId = userID,
                OrderId = result.OrderId,
            }, opt => opt.Excluding(f => f.Business).Excluding(f => f.OrderItems));

            order.OrderItems.First().Should().BeEquivalentTo(new OrderItem()
            {
                BookId = book.BookId,
                OrderId = result.OrderId,
                Price = order.OrderItems.First().Price,
                Quantity = order.OrderItems.First().Quantity,
            }, opt => opt.Excluding(f => f.OrderItemId).Excluding(f => f.Order).Excluding(f => f.Book));
        }
    }
}
