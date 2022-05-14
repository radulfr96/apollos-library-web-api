using ApollosLibrary.Application.Order.Commands.AddOrderCommand;
using ApollosLibrary.Application.Order.Commands.DeleteOrderCommand;
using ApollosLibrary.Application.Order.Commands.UpdateOrderCommand;
using ApollosLibrary.Application.Order.Queries.GetOrderQuery;
using ApollosLibrary.Application.Order.Queries.GetOrdersQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ApollosLibrary.WebApi.Controllers
{
    /// <summary>
    /// Used to manage a users orders
    /// </summary>
    [Route("api/[controller]")]
    public class OrderController : BaseApiController
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator, IConfiguration configuration) : base(configuration)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Used to add an order
        /// </summary>
        /// <param name="command">The command containing the data for the order</param>
        /// <returns>Response containing the resulting id</returns>
        [HttpPost("")]
        public async Task<AddOrderCommandDto> AddOrder([FromBody] AddOrderCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to get a users orders
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("")]
        public async Task<GetOrdersQueryDto> GetOrders()
        {
            return await _mediator.Send(new GetOrdersQuery());
        }

        /// <summary>
        /// Used to get an order
        /// </summary>
        /// <param name="id">The id of the order to be retreived</param>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("{id}")]
        public async Task<GetOrderQueryDto> GetLibraryEntry([FromRoute]int id)
        {
            return await _mediator.Send(new GetOrderQuery() { OrderId = id });
        }

        /// <summary>
        /// Used to update an order
        /// </summary>
        /// <param name="command">The information used to update the order</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPut("")]
        public async Task<UpdateOrderCommandDto> UpdateLibraryEntry([FromBody] UpdateOrderCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to remove an order from the system
        /// </summary>
        /// <param name="id">The id of the order to be deleted</param>
        /// <returns>Response that indicates the result</returns>
        [HttpDelete("{id}")]
        public async Task<DeleteOrderCommandDto> DeleteLibraryEntry([FromRoute] int id)
        {
            return await _mediator.Send(new DeleteOrderCommand() { OrderId = id });
        }
    }
}
