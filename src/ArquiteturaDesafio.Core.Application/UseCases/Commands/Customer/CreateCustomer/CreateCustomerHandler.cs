using AutoMapper;

using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using MediatR;
using ArquiteturaDesafio.Core.Domain.Enum;
using System.Threading;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Customer.CreateCustomer;

public class CreateCustomerHandler :
       IRequestHandler<CreateCustomerRequest, CreateCustomerResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IJwtTokenService _tokenService;

    public CreateCustomerHandler(IUnitOfWork unitOfWork,
        ICustomerRepository customerRepository,
        IMapper mapper,
        IJwtTokenService tokenService
        )
    {
        _unitOfWork = unitOfWork;
        _customerRepository = customerRepository;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    public async Task<CreateCustomerResponse> Handle(CreateCustomerRequest request,
        CancellationToken cancellationToken)
    {

        var customer = _mapper.Map<ArquiteturaDesafio.Core.Domain.Entities.Customer>(request);
        _customerRepository.Create(customer);
        await _unitOfWork.Commit(cancellationToken);
        return _mapper.Map<CreateCustomerResponse>(customer);
    }
}
