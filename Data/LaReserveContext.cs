using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class LaReserveContext : DbContext
{
    public DbSet<Animal> Animaux { get; set; } = null!;
    public DbSet<Personnel> Personnels { get; set; } = null!;
    public DbSet<Lieu> Lieux { get; set; } = null!;
    public DbSet<Evenement> Evenements { get; set; } = null!;

    public string DbPath { get; private set; }

    public LaReserveContext()
    {
        // Path to SQLite database file
        DbPath = "LaReserve.db";
    }

    // The following configures EF to create a SQLite database file locally
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // Use SQLite as database
        options.UseSqlite($"Data Source={DbPath}");
        // Optional: log SQL queries to console
        //options.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
    }
}
