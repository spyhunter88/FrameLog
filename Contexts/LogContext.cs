using FrameLog.Models;
using System.Data.Entity;
using FrameLog.Helpers;

namespace FrameLog.Contexts
{

    public class LogContext : DbContext, IHistoryContext
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

        #region Implement IHistoryContext
        public bool ObjectHasReference(object model)
        {
            return DataContextHelper.ObjectHasReference(model);
        }

        public string GetReferenceForObject(object model)
        {
            return DataContextHelper.GetReferenceForObject(model);
        }

        public string GetReferencePropertyForObject(object model)
        {
            return DataContextHelper.GetReferencePropertyForObject(model);
        }

        public object GetObjectByReference(System.Type type, string raw)
        {
            return DataContextHelper.GetObjectByReference(this, type, raw);
        }
        #endregion
    }
}
