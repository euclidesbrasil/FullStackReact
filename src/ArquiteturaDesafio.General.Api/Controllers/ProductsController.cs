using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ArquiteturaDesafio.Application.UseCases.Commands.Product.UpdateProduct;
using ArquiteturaDesafio.Application.UseCases.Commands.Product.CreateProduct;
using ArquiteturaDesafio.Application.UseCases.Commands.Product.DeleteProduct;
using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetProductById;
using ArquiteturaDesafio.Application.UseCases.Queries.GetProductsQuery;

namespace ArquiteturaDesafio.General.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(201)] // Criação
    [ProducesResponseType(400)] // Erro de validação
    [ProducesResponseType(401)] // Autenticação
    [ProducesResponseType(500)] // Erro interno
    public async Task<ActionResult<CreateProductResponse>> Create(CreateProductRequest request,
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
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request,
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
        await _mediator.Send(new DeleteProductRequest(id), cancellationToken);
        return NoContent();
    }

    [HttpGet("/Products/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetProductByIdRequest(id), cancellationToken);
        return Ok(response);
    }

    [HttpGet("/Products")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<List<GetProductsQueryResponse>>> GetCustomersQuery(CancellationToken cancellationToken, int _page = 1, int _size = 10, [FromQuery] Dictionary<string, string> filters = null, string _order = "id asc")
    {
        filters = filters ?? new Dictionary<string, string>();
        filters = HttpContext.Request.Query
        .Where(q => q.Key != "_page" && q.Key != "_size" && q.Key != "_order")
        .ToDictionary(q => q.Key, q => q.Value.ToString());

        var response = await _mediator.Send(new GetProductsQueryRequest(_page, _size, _order, filters), cancellationToken);
        return Ok(response);
    }

}
