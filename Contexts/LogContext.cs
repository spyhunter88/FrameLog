using FrameLog.Models;
using System.Data.Entity;

namespace FrameLog.Contexts
{

    public class LogContext : DbContext
    {

        public LogContext()
            : base("LogContext") { }

        public DbSet<User> Users { get; set; }
        public DbSet<ChangeSet> ChangeSets { get; set; }
        public DbSet<ObjectChange> ObjectChanges { get; set; }
        public DbSet<PropertyChange> PropertyChanges { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("AspNetUsers");
            modelBuilder.Entity<ChangeSet>().ToTable("ChangeSets");
            modelBuilder.Entity<ObjectChange>().ToTable("ObjectChanges");
            modelBuilder.Entity<PropertyChange>().ToTable("PropertyChanges");
        }
    }
}
