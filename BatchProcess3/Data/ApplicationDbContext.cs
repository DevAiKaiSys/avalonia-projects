using Microsoft.EntityFrameworkCore;

namespace BatchProcess3.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<SettingsDataModel> Settings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=settings.db");
    }
}