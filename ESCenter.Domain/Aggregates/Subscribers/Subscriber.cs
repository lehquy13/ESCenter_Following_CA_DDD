using Matt.SharedKernel.Domain.Primitives.Auditing;

namespace ESCenter.Domain.Aggregates.Subscribers;
public class Subscriber : CreationAuditedAggregateRoot<int>
{
    public string Email { get; private set; } = null!;

    private Subscriber()
    {
    }
    
    public static Subscriber Create(string email)
    {
        return new Subscriber()
        {
            Email = email
        };
    }
   
}

