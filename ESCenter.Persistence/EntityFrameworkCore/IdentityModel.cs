using Microsoft.AspNetCore.Identity;

namespace ESCenter.Persistence.EntityFrameworkCore;

public class EsIdentityUser : IdentityUser;

public class EsIdentityRole(string name) : IdentityRole(name);