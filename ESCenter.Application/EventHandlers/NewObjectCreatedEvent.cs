using ESCenter.Domain.Shared.NotificationConsts;
using MediatR;

namespace ESCenter.Application.EventHandlers;

public record NewObjectCreatedEvent(string ObjectId, string Message, NotificationEnum NotificationEnum) : INotification;

