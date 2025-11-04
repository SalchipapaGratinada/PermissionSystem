using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PermissionSystem.Contracts.DTOs.Permissions;
using PermissionSystem.Domain.Entities;

namespace PermissionSystem.Application.Services.Interfaces
{
    public interface IPermissionService
    {
        /// <summary>
        /// Get all permissions
        /// </summary>
        /// <returns> A list of PermissionDto</returns>
        Task<IEnumerable<PermissionDto>> GetAllAsync();

        /// <summary>
        /// Get permission by id
        /// </summary>
        /// <param name="id"> Permission Id</param>
        /// <returns> Returns PermissionDto if found, otherwise null</returns>
        Task<PermissionDto?> GetByIdAsync(int id);

        /// <summary>
        /// Create a new permission
        /// </summary>
        /// <param name="dto"> PermissionDto to create</param>
        /// <returns> Created PermissionDto</returns>
        Task<PermissionDto> CreateAsync(PermissionDto dto);

        /// <summary>
        /// Update an existing permission
        /// </summary>
        /// <param name="dto"> PermissionDto to update</param>
        /// <returns> True if update was successful, otherwise false</returns>
        Task<bool> UpdateAsync(PermissionDto dto);

        /// <summary>
        /// Delete a permission by id
        /// </summary>
        /// <param name="id"> Permission Id</param>
        /// <returns> True if delete was successful, otherwise false</returns>
        Task<bool> DeleteAsync(int id);
    }
}
