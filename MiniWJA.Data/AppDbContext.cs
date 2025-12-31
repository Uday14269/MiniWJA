using MiniWJA.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Contexts;
using System.Data.Entity;

namespace MiniWJA.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=MiniWJA")
        {
            try
            {
                // Optionally test the database connection here if needed
                // Database.Connection.Open();
                // Database.Connection.Close();
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error initializing AppDbContext: {ex.Message}");
                throw new InvalidOperationException("Failed to initialize the database context.", ex);
            }
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<SuppliesStatus> SuppliesStatuses { get; set; }
        public DbSet<UsageCounters> UsageCounters { get; set; }
        public DbSet<DiscoveryRange> DiscoveryRanges { get; set; }
        public DbSet<ConfigTemplate> ConfigTemplates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            try
            {
                modelBuilder.Entity<Device>()
                            .HasRequired(d => d.Supplies)
                            .WithRequiredPrincipal()
                            .WillCascadeOnDelete(true);

                modelBuilder.Entity<Device>()
                            .HasRequired(d => d.Counters)
                            .WithRequiredPrincipal()
                            .WillCascadeOnDelete(true);

                base.OnModelCreating(modelBuilder);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error in OnModelCreating: {ex.Message}");
                throw new InvalidOperationException("Failed during model creation.", ex);
            }
        }
    }
}
