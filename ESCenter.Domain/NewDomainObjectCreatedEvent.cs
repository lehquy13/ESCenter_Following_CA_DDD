using ESCenter.Domain.Shared.NotificationConsts;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Domain;

public record NewDomainObjectCreatedEvent(string ObjectId, string Message, NotificationEnum NotificationEnum) : IDomainEvent;