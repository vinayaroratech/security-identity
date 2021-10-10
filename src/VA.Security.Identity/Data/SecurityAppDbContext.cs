using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VA.Security.Identity.Data
{
    internal class SecurityAppDbContext : IdentityDbContext
    {
        public SecurityAppDbContext(DbContextOptions<SecurityAppDbContext> options) : base(options) { }
    }
}