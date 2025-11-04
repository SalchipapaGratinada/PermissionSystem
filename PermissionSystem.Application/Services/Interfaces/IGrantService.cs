using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PermissionSystem.Contracts.DTOs.Grants;
using PermissionSystem.Contracts.DTOs.Permissions;
using PermissionSystem.Domain.Entities;

namespace PermissionSystem.Application.Services.Interfaces
{
    public interface IGrantService
    {
        /// <summary>
        /// Gets all grants.
        /// </summary>
        /// <returns> A list of GrantDto objects.</returns>
        Task<IEnumerable<GrantDto>> GetAllAsync();

        /// <summary>
        /// Gets a grant by its ID.
        /// </summary>
        /// <param name="id"> The ID of the grant.</param>
        /// <returns> A GrantDto object or null if not found.</returns>
        Task<GrantDto?> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new grant.
        /// </summary>
        /// <param name="dto"> The GrantDto object containing grant details.</param>
        /// <returns> The created GrantDto object.</returns>
        Task<GrantDto> CreateAsync(GrantDto dto);

        /// <summary>
        /// Updates an existing grant.
        /// </summary>
        /// <param name="dto"> The GrantDto object containing updated grant details.</param>
        /// <returns> True if the update was successful, otherwise false.</returns>
        Task<bool> UpdateAsync(GrantDto dto);

        /// <summary>
        /// Deletes a grant by its ID.
        /// </summary>
        /// <param name="id"> The ID of the grant.</param>
        /// <returns> True if deleted, false if not found.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Gets permissions assigned to a user.
        /// </summary>
        /// <param name="userId"> The ID of the user.</param>
        /// <returns> A list of PermissionDto objects.</returns>
        Task<IEnumerable<PermissionDto>> GetPermissionsByUserAsync(int userId);

        /// <summary>
        /// Gets permissions assigned to a hierarchy node.
        /// </summary>
        /// <param name="hierarchyNodeId"> The ID of the hierarchy node.</param>
        /// <returns> A list of PermissionDto objects.</returns>
        Task<IEnumerable<PermissionDto>> GetPermissionsByHierarchyAsync(int hierarchyNodeId);
    }
}
