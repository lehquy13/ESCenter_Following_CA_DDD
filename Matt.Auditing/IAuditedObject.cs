namespace Matt.Auditing;

/// <summary>
/// This interface can be implemented to add standard auditing properties to a class.
/// </summary>
public interface IAuditedObject : ICreationAuditedObject, IModificationAuditedObject
{
}