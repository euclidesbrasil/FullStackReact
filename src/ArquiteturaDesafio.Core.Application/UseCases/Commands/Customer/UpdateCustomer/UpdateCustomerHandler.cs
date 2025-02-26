using AutoMapper;

using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Customer.UpdateCustomer;

public class UpdateCustomerHandler :
       IRequestHandler<UpdateCustomerRequest, UpdateCustomerResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public UpdateCustomerHandler(IUnitOfWork unitOfWork,
        ICustomerRepository customerRepository,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<UpdateCustomerResponse> Handle(UpdateCustomerRequest request,
        CancellationToken cancellationToken)
    {

        var custumer = await _customerRepository.Get(request.Id, cancellationToken);
        if(custumer is null)
        {
            throw new KeyNotFoundException("Cliente não encontrado");
        }
        custumer.Update(request.Name, new Core.Domain.ValueObjects.InfoContact(request.Identification.Email, request.Identification.Phone));
        await _unitOfWork.Commit(cancellationToken);
        return new UpdateCustomerResponse("Cliente salvo com sucesso");
    }
}
