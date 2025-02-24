using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ArquiteturaDesafio.Application.UseCases.Commands.Order.CreateSale;
using ArquiteturaDesafio.Application.UseCases.Commands.Order.UpdateSale;
using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetOrdersQuery;
using ArquiteturaDesafio.Application.UseCases.Queries.GetOrderById;
using ArquiteturaDesafio.Application.UseCases.Commands.Order.DeleteOrder;
namespace ArquiteturaDesafio.General.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(201)] // Criação
    [ProducesResponseType(400)] // Erro de validação
    [ProducesResponseType(401)] // Autenticação
    [ProducesResponseType(500)] // Erro interno
    public async Task<ActionResult<CreateOrderResponse>> Create(CreateOrderRequest request,
                                                         CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return CreatedAtAction("Create", new { id = response.id }, response);
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrderRequest request,
                                            CancellationToken cancellationToken)
    {
        request.SetId(id);
        await _mediator.Send(request, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Delete(Guid id,
                                            CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteOrderRequest(id), cancellationToken);
        return NoContent();
    }


    [HttpGet("/Orders/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<GetOrderByIdResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetOrderByIdRequest(id), cancellationToken);
        return Ok(response);
    }

    [HttpGet("/Orders")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<GetOrdersQueryResponse>> GetSalesQuery(CancellationToken cancellationToken, int _page = 1, int _size = 10, [FromQuery] Dictionary<string, string> filters = null, string _order = "id asc")
    {
        filters = filters ?? new Dictionary<string, string>();
        filters = HttpContext.Request.Query
            .Where(q => q.Key != "_page" && q.Key != "_size" && q.Key != "_order")
            .ToDictionary(q => q.Key, q => q.Value.ToString());

        var response = await _mediator.Send(new GetOrdersQueryRequest( _page, _size, _order, filters), cancellationToken);
        return Ok(response);
    }
}
