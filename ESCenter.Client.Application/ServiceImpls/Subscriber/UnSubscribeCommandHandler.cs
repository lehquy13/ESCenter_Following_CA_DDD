using ESCenter.Domain.DomainServices.Interfaces;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Client.Application.ServiceImpls.Subscriber;

public class UnSubscribeCommandHandler(
    ISubscribeDomainService subscribeDomainService,
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger) : CommandHandlerBase<UnSubscribeCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(UnSubscribeCommand command, CancellationToken cancellationToken)
    {
        return await subscribeDomainService.UnSubscribe(command.Email);
    }
}