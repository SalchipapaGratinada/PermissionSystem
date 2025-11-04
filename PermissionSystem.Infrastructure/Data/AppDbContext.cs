using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PermissionSystem.Domain.Entities;

namespace PermissionSystem.Infrastructure.Data
{
    /// <summary>
    /// Application database context.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Constructor for AppDbContext.
        /// </summary>
        /// <param name="options"></param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        /// <summary>
        /// DbSet for Users.
        /// </summary>
        public DbSet<User> Users => Set<User>();

        /// <summary>
        /// DbSet for HierarchyNodes.
        /// </summary>
        public DbSet<HierarchyNode> HierarchyNodes => Set<HierarchyNode>();

        /// <summary>
        /// DbSet for Permissions.
        /// </summary>
        public DbSet<Permission> Permissions => Set<Permission>();

        /// <summary>
        /// DbSet for Grants.
        /// </summary>
        public DbSet<Grant> Grants => Set<Grant>();

        /// <summary>
        /// DbSet for Notifications.
        /// </summary>
        public DbSet<Notification> Notifications => Set<Notification>();

        /// <summary>
        /// Configure the model relationships.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación jerárquica entre nodos (auto-referencia)
            modelBuilder.Entity<HierarchyNode>()
                .HasMany(h => h.Children)
                .WithOne(h => h.Parent)
                .HasForeignKey(h => h.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación entre HierarchyNode y User
            modelBuilder.Entity<HierarchyNode>()
                .HasMany(h => h.Users)
                .WithOne(u => u.HierarchyNode)
                .HasForeignKey(u => u.HierarchyNodeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación entre Grant y User
            modelBuilder.Entity<Grant>()
                .HasOne(g => g.User)
                .WithMany(u => u.Grants)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Grant y HierarchyNode
            modelBuilder.Entity<Grant>()
                .HasOne(g => g.HierarchyNode)
                .WithMany()
                .HasForeignKey(g => g.HierarchyNodeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
