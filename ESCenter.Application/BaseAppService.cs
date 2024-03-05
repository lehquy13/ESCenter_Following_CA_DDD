using MapsterMapper;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Application;

public abstract class BaseAppService<T> where T : BaseAppService<T>
{
    protected readonly IMapper Mapper;

    protected readonly IUnitOfWork UnitOfWork;

    //protected readonly IAppCache _cache;
    //protected readonly IPublisher _publisher;
    protected readonly IAppLogger<BaseAppService<T>> Logger;

    public BaseAppService(IMapper mapper, IUnitOfWork unitOfWork, IAppLogger<BaseAppService<T>> logger)
    {
        Mapper = mapper;
        UnitOfWork = unitOfWork;
        Logger = logger;
    }
}

public abstract class BaseCommandAppService<T> : BaseAppService<T>
    where T : BaseCommandAppService<T>
{
    //protected readonly IAppCache _cache;
    protected readonly IPublisher _publisher;

    public BaseCommandAppService(IMapper mapper, IUnitOfWork unitOfWork, IAppLogger<BaseAppService<T>> logger,
        IPublisher publisher)
        : base(mapper, unitOfWork, logger)
    {
        _publisher = publisher;
    }
}