using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PriceNegotiationApp.Data;
using PriceNegotiationApp.Models;

namespace PriceNegotiationApp.Tests.InMemoryDb;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected  override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
            {
                var dbContextOptions = services
                    .Where(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>) || 
                                d.ServiceType.FullName!.Contains("EntityFramework"))
                    .ToList();

                foreach (var d in dbContextOptions)
                {
                    services.Remove(d);
                }

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDb");
                });
                    
                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();

                db.Database.EnsureCreated();

                var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();

                if (!userManager.Users.Any())
                {
                    var testUser = new ApplicationUser
                    {
                        Fullname = "Test User",
                        Email = "testuser@email.com",
                        UserName = "testuser@email.com"
                    };

                    var result = userManager.CreateAsync(testUser, "TestPassword123!").Result;
                }
            }
        );
    }
}