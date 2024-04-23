using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public class AppDbContext: DbContext
{
    public virtual DbSet<Ticket> Tickets { get; set; } = default!;


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}