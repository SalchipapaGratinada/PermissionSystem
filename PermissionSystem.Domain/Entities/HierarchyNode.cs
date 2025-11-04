using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionSystem.Domain.Entities
{
    /// <summary>
    /// Represents a node in a hierarchical structure.
    /// </summary>
    public class HierarchyNode
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int? ParentId { get; set; }
        public HierarchyNode? Parent { get; set; }

        public ICollection<HierarchyNode> Children { get; set; } = new List<HierarchyNode>();
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
