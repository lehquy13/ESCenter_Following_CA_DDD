using MediatR;

namespace Matt.SharedKernel.Domain;

public record NewObjectCreatedEvent(string ObjectId, string Message) : INotification;

