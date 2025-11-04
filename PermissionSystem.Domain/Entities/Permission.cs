using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionSystem.Domain.Entities
{
    /// <summary>
    /// Represents a permission within the permission system.
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// Gets or sets the unique identifier for the permission.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the code of the permission.
        /// </summary>
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the permission.
        /// </summary>
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Grant> Grants { get; set; } = new List<Grant>();
    }
}
