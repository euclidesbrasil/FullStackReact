using ArquiteturaDesafio.Application.UseCases.Commands.Customer.CreateCustomer;
using ArquiteturaDesafio.Application.UseCases.Commands.Customer.DeleteCustomer;
using ArquiteturaDesafio.Application.UseCases.Commands.Customer.UpdateCustomer;
using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetCustomerById;
using ArquiteturaDesafio.Application.UseCases.Queries.GetCustomersQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ArquiteturaDesafio.General.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CreateCustomerResponse>> Create(CreateCustomerRequest request,
                                                             CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return CreatedAtAction("Create", new { id = response.id }, response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerRequest request,
                                                CancellationToken cancellationToken)
        {
            request.SetId(id);
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id,
                                                CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteCustomerRequest(id), cancellationToken);
            return NoContent();
        }

        [HttpGet("/Customers/{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetCustomerByIdRequest(id), cancellationToken);
            return Ok(response);
        }

        [HttpGet("/Customers")]
        public async Task<ActionResult<List<GetCustomersQueryResponse>>> GetCustomersQuery(CancellationToken cancellationToken, int _page = 1, int _size = 10, [FromQuery] Dictionary<string, string> filters = null, string _order = "id asc")
        {
            filters = filters ?? new Dictionary<string, string>();
            filters = HttpContext.Request.Query
            .Where(q => q.Key != "_page" && q.Key != "_size" && q.Key != "_order")
            .ToDictionary(q => q.Key, q => q.Value.ToString());

            var response = await _mediator.Send(new GetCustomersQueryRequest(_page, _size, _order, filters), cancellationToken);
            return Ok(response);
        }
    }
}
