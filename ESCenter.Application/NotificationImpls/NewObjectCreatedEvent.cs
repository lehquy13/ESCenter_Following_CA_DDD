using ESCenter.Domain.Shared.NotificationConsts;
using MediatR;

namespace ESCenter.Application.NotificationImpls;

internal record NewObjectCreatedEvent(string ObjectId, string Message, NotificationEnum NotificationEnum) : INotification;

