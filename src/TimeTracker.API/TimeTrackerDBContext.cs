using Microsoft.EntityFrameworkCore;

public class TimeTrackerDBContext : DbContext
{
  public DbSet<User> Users { get; set; }
  public DbSet<WorkEvent> WorkEvents { get; set; }

  public string DbPath { get; }

  public TimeTrackerDBContext(DbContextOptions<TimeTrackerDBContext> options)
  {
    var folder = Environment.SpecialFolder.LocalApplicationData;
    var path = Environment.GetFolderPath(folder);
    DbPath = Path.Join(path, "time_tracker.db");
  }

  // The following configures EF to create a Sqlite database file in the
  // special "local" folder for your platform.
  protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlite($"Data Source={DbPath}");
}