using Microsoft.AspNetCore.Identity;

namespace ESCenter.Persistence.Entity_Framework_Core;

public class EsIdentityUser : IdentityUser<Guid>;

public class EsIdentityRole(string name) : IdentityRole<Guid>(name);