using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PriceNegotiationApp.Models;

namespace PriceNegotiationApp.Data
{
    /// <summary>
    /// Represents the application's database context, including the Identity and business models.
    /// </summary>
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to configure the database context.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        /// <summary>
        /// Gets or sets the collection of <see cref="Product"/> entities.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Negotiation"/> entities.
        /// </summary>
        public DbSet<Negotiation> Negotiations { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="PriceProposal"/> entities.
        /// </summary>
        public DbSet<PriceProposal> PriceProposals { get; set; }
    }
}