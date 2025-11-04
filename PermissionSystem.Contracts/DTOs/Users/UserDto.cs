using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionSystem.Contracts.DTOs.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Blood { get; set; } = string.Empty;

        public int? HierarchyNodeId { get; set; }

        public string? HierarchyNodeName { get; set; }
    }
}
