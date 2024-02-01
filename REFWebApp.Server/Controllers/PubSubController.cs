using MediatR;
using REFWebApp.Server.Queries;
using Microsoft.AspNetCore.Mvc;
using REFWebApp.Server.Commands;

namespace REFWebApp.Server.Controllers
{
    [Route("api/pubsub")]
    [ApiController]
    public class PubSubController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PubSubController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            var products = await _mediator.Send(new GetProductsQuery());
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] Product product)
        {
            {
                await _mediator.Send(new AddProductCommand(product));
                return StatusCode(201);
            }
        }
    }
}
