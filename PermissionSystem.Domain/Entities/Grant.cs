using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionSystem.Domain.Entities
{
    /// <summary>
    /// Represents a grant within the permission system.
    /// </summary>
    public class Grant
    {
        /// <summary>
        /// Gets or sets the unique identifier for the grant.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the permission associated with the grant.
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the permission associated with the grant.
        /// </summary>
        public Permission Permission { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the user associated with the grant.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the user associated with the grant.
        /// </summary>
        public User? User { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the hierarchy node associated with the grant.
        /// </summary>
        public int? HierarchyNodeId { get; set; }

        /// <summary>
        /// Gets or sets the hierarchy node associated with the grant.
        /// </summary>
        public HierarchyNode? HierarchyNode { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the grant was assigned.
        /// </summary>
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
