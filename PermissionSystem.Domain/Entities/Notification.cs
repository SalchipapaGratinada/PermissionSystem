using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PermissionSystem.Domain.Entities
{
    /// <summary>
    /// Represents a notification within the permission system.
    /// </summary>
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;

        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        public int? HierarchyNodeId { get; set; }

        [ForeignKey(nameof(HierarchyNodeId))]
        public HierarchyNode? HierarchyNode { get; set; }
    }
}
