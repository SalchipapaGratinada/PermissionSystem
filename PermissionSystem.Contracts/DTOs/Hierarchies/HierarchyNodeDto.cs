using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PermissionSystem.Contracts.DTOs.Users;

namespace PermissionSystem.Contracts.DTOs.Hierarchies
{
    public class HierarchyNodeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int? ParentId { get; set; }
        public string? ParentName { get; set; }

        public List<UserDto> Users { get; set; } = new();
    }
}
