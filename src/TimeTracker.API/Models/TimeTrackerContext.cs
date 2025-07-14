using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class TimeTrackerContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<WorkEvent> WorkEvents { get; set; }

    public string DbPath { get; }

    public TimeTrackerContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "time_tracker.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}