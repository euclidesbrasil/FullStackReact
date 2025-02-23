using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
namespace ArquiteturaDesafio.Application.UseCases.Commands.Customer.UpdateCustomer;

public class UpdateCustomerResponse : CustomerDTO
{
    public int Id { get; set; }
}
