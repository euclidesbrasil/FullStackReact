using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ArquiteturaDesafio.Application.UseCases.Commands.Product.UpdateProduct;
using ArquiteturaDesafio.Application.UseCases.Commands.Product.CreateProduct;
using ArquiteturaDesafio.Application.UseCases.Commands.Product.DeleteProduct;

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
        return CreatedAtAction("Create", new { id = response.Id }, response);

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

}
