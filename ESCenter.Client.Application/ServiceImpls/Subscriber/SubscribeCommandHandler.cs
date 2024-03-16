using ESCenter.Domain.DomainServices.Interfaces;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Client.Application.ServiceImpls.Subscriber;

public class SubscribeCommandHandler(
    ISubscribeDomainService subscribeDomainService,
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger) : CommandHandlerBase<SubscribeCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(SubscribeCommand command, CancellationToken cancellationToken)
    {
        return await subscribeDomainService.Subscribe(command.Email);
    }
}