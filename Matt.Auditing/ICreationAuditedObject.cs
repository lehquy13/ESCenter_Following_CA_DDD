namespace Matt.Auditing;

/// <summary>
/// This interface can be implemented to store creation information (who and when created).
/// </summary>
public interface ICreationAuditedObject : IHasCreationTime
{
    string? CreatorId { get; }
}