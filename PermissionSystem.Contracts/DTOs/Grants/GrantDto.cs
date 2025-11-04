using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionSystem.Contracts.DTOs.Grants
{
    public class GrantDto
    {
        public int Id { get; set; }
        public int PermissionId { get; set; }

        public string PermissionCode { get; set; } = string.Empty;

        public string PermissionDescripcion { get; set; } = string.Empty;

        public int? UserId { get; set; }
        public string? Username { get; set; }

        public int? HierarchyNodeId { get; set; }
        public string? HierarchyNodeName { get; set; }
    }
}
