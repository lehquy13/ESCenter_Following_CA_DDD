namespace Matt.Auditing;

/// <summary>
/// Adds user navigation properties to <see cref="IFullAuditedObject"/> interface for user.
/// </summary>
public interface IFullAuditedObject : IAuditedObject, IDeletionAuditedObject
{
}