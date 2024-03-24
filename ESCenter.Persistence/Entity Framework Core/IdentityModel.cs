using Microsoft.AspNetCore.Identity;

namespace ESCenter.Persistence.Entity_Framework_Core;

public class EsIdentityUser : IdentityUser;

public class EsIdentityRole(string name) : IdentityRole(name);