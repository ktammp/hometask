using DAL.EF;
using Domain;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IntegrationTests;

public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup: class
{
    private static bool _dbInitialized = false;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // find DbContext
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<AppDbContext>));

            // if found - remove
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // and new DbContext
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });
            
            services.AddAntiforgery(t =>
            {
                t.Cookie.Name = AntiForgeryTokenController.AntiForgeryCookieName;
                t.FormFieldName = AntiForgeryTokenController.AntiForgeryFieldName;
            });

            // create db and seed data
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();
            var logger = scopedServices
                .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

            db.Database.EnsureCreated();


            try
            { 
                if (_dbInitialized == false)
                {
                    _dbInitialized = true;
                    if (db.Tickets.Any()) return;
                    db.Tickets.Add(new Ticket() {
                        Id = Guid.ParseExact("bddd91db-6d50-4bdd-b13b-377d7497ede0", "D"),
                        Description = "Testing Testing",
                        Deadline = DateTime.Now});
                    db.Tickets.Add(new Ticket() {
                        Id = Guid.ParseExact("bddd91db-6d50-4bdd-b13b-111d7497ede0", "D"),
                        Description = "Resolved ticket",
                        Deadline = DateTime.Now,
                        Resolved = true
                    });
                    db.SaveChanges();
                    
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the " +
                                    "database with test messages. Error: {Message}", ex.Message);
            }
        });
    }
}
   
