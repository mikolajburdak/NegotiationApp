using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PriceNegotiationApp.Models;

namespace PriceNegotiationApp.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Negotiation> Negotiations { get; set; }
        public DbSet<PriceProposal> PriceProposals { get; set; }

    }
}
