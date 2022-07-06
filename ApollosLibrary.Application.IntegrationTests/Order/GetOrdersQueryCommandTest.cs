using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Order.Queries.GetOrdersQuery;
using ApollosLibrary.Domain;
using ApollosLibrary.Domain.Enums;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NodaTime;
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
    public class GetOrdersQueryCommandTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IDateTimeService _dateTimeService;

        public GetOrdersQueryCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(LocalDateTime.FromDateTime(new DateTime(2021, 02, 07)));
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

            var order1 = new Domain.Order()
            {
                Business = business1,
                OrderDate = _dateTimeService.Now,
                UserId = userID,
                OrderItems = new List<OrderItem>()
                {
                    new OrderItem()
                    {
                        Book = book1,
                        Price = 15.00m,
                        Quantity = 2,
                    },
                    new OrderItem()
                    {
                        Book = book2,
                        Price = 10.00m,
                        Quantity = 1,
                    },
                },
            };

            var order2 = new Domain.Order()
            {
                Business = business2,
                OrderDate = _dateTimeService.Now,
                UserId = userID,
                OrderItems = new List<OrderItem>()
                {
                    new OrderItem()
                    {
                        Book = book2,
                        Price = 15.00m,
                        Quantity = 3,
                    },
                    new OrderItem()
                    {
                        Book = book1,
                        Price = 20.00m,
                        Quantity = 2,
                    },
                },
            };

            _context.Orders.Add(order1);
            _context.Orders.Add(order2);

            _context.SaveChanges();

            var command = new GetOrdersQuery()
            {
            };

            var result = await _mediatr.Send(command);

            result.Should().NotBeNull();
            result.Orders.Should().HaveCount(2);
            result.Should().BeEquivalentTo(new GetOrdersQueryDto()
            {
                Orders = new List<OrderListItem>()
                {
                    new OrderListItem()
                    {
                        Bookshop = business1.Name,
                        OrderDate = _dateTimeService.Now,
                        NumberOfItems = 2,
                        OrderId = order1.OrderId,
                        Total = (order1.OrderItems[0].Quantity * order1.OrderItems[0].Price) + (order1.OrderItems[1].Quantity * order1.OrderItems[1].Price),
                    },
                    new OrderListItem()
                    {
                        Bookshop = business2.Name,
                        OrderDate = _dateTimeService.Now,
                        NumberOfItems = 2,
                        OrderId = order2.OrderId,
                        Total = (order2.OrderItems[0].Quantity * order2.OrderItems[0].Price) + (order2.OrderItems[1].Quantity * order2.OrderItems[1].Price),
                    },
                }
            });
        }
    }
}
