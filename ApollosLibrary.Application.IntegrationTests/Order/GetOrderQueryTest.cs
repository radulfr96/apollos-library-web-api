using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Order;
using ApollosLibrary.Application.Order.Queries.GetOrderQuery;
using ApollosLibrary.Domain;
using ApollosLibrary.Domain.Enums;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
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
    public class GetOrderQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IDateTimeService _dateTimeService;

        public GetOrderQueryTest(TestFixture fixture) : base(fixture)
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
        public async Task GetOrderQuery()
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
                    },
                    new OrderItem()
                    {
                        Book = book2,
                        Price = 10.00m,
                        Quantity = 1,
                    },
                },
            };

            _context.Orders.Add(order);

            _context.SaveChanges();

            var command = new GetOrderQuery()
            {
                OrderId = order.OrderId,
            };

            var result = await _mediatr.Send(command);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new GetOrderQueryDto()
            {
                BusinessId = order.BusinessId,
                OrderDate = order.OrderDate,
                OrderId = order.OrderId,
            }, opt => opt.Excluding(f => f.OrderItems));

            result.OrderItems[0].Should().BeEquivalentTo(new OrderItemDTO()
            {
                BookId = book1.BookId,
                UnitPrice = order.OrderItems[0].Price,
                ISBN = book1.Isbn,
                eISBN = book1.EIsbn,
                Quantity = order.OrderItems[0].Quantity,
                Title = book1.Title,
            });

            result.OrderItems[1].Should().BeEquivalentTo(new OrderItemDTO()
            {
                BookId = book2.BookId,
                UnitPrice = order.OrderItems[1].Price,
                ISBN = book2.Isbn,
                eISBN = book2.EIsbn,
                Quantity = order.OrderItems[1].Quantity,
                Title = book2.Title,
            });
        }
    }
}
