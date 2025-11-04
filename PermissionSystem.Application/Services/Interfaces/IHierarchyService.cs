using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PermissionSystem.Contracts.DTOs.Hierarchies;
using PermissionSystem.Domain.Entities;

namespace PermissionSystem.Application.Services.Interfaces
{
    public interface IHierarchyService
    {
        /// <summary>
        /// Gets all hierarchy nodes.
        /// </summary>
        /// <returns> A list of HierarchyNodeDto objects.</returns>
        Task<IEnumerable<HierarchyNodeDto>> GetAllAsync();

        /// <summary>
        /// Gets a hierarchy node by its ID.
        /// </summary>
        /// <param name="id"> The ID of the hierarchy node.</param>
        /// <returns> A HierarchyNodeDto object or null if not found.</returns>
        Task<HierarchyNodeDto?> GetByIdAsync(int id);

        /// <summary>
        /// Gets children of a hierarchy node by its parent ID.
        /// </summary>
        /// <param name="parentId"> The ID of the parent hierarchy node.</param>
        /// <returns> A list of HierarchyNodeDto objects.</returns>
        Task<IEnumerable<HierarchyNodeDto>> GetChildrenAsync(int parentId);

        /// <summary>
        /// Creates a new hierarchy node.
        /// </summary>
        /// <param name="node"> The HierarchyNode entity containing node details.</param>
        /// <returns> The created HierarchyNodeDto object.</returns>
        Task<HierarchyNodeDto> CreateAsync(HierarchyNode node);

        /// <summary>
        /// Updates an existing hierarchy node.
        /// </summary>
        /// <param name="node"> The HierarchyNode entity containing updated node details.</param>
        /// <returns> The updated HierarchyNodeDto object.</returns>
        Task<HierarchyNodeDto> UpdateAsync(HierarchyNode node);

        /// <summary>
        /// Deletes a hierarchy node by its ID.
        /// </summary>
        /// <param name="id"> The ID of the hierarchy node.</param>
        /// <returns> True if deleted, false if not found.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
