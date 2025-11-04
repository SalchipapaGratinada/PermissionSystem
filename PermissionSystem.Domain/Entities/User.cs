using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionSystem.Domain.Entities
{
    /// <summary>
    /// Represents a user within the permission system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string Apellido { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the blood type of the user.
        /// </summary>
        public string TipoSangre { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the hierarchy node associated with the user.
        /// </summary>
        public int? HierarchyNodeId { get; set; }

        /// <summary>
        /// Gets or sets the hierarchy node associated with the user.
        /// </summary>
        public HierarchyNode? HierarchyNode { get; set; }

        /// <summary>
        /// Gets or sets the collection of grants associated with the user.
        /// </summary>
        public ICollection<Grant> Grants { get; set; } = new List<Grant>();

        /// <summary>
        /// Gets or sets the username for authentication.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password for authentication.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
