using ESCenter.Domain.Shared.NotificationConsts;
using MediatR;

namespace ESCenter.Application.EventHandlers;

internal record NewObjectCreatedEvent(string ObjectId, string Message, NotificationEnum NotificationEnum) : INotification;

