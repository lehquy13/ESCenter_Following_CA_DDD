namespace Matt.Auditing;

/// <summary>
/// This interface can be implemented to store deletion information (who delete and when deleted).
/// </summary>
public interface IDeletionAuditedObject : IHasDeletionTime
{
    /// <summary>
    /// Id of the deleter user.
    /// </summary>
    string? DeleterId { get; }
}