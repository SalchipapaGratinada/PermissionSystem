using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PermissionSystem.Contracts.DTOs.Users;
using PermissionSystem.Domain.Entities;

namespace PermissionSystem.Application.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns> A list of UserDto</returns>
        Task<IEnumerable<UserDto>> GetAllAsync();

        /// <summary>
        /// Get user by id 
        /// </summary>
        /// <param name="id"> User Id</param>
        /// <returns> Returns UserDto if found, otherwise null</returns>
        Task<UserDto?> GetByIdAsync(int id);

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user"> User entity to create</param>
        /// <returns> Created UserDto</returns>
        Task<UserDto> CreateAsync(User user);

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="user"> User entity to update</param>
        /// <returns> Updated UserDto</returns>
        Task<UserDto> UpdateAsync(User user);

        /// <summary>
        /// Delete a user by id
        /// </summary>
        /// <param name="id"> User Id</param>
        /// <returns> True if delete was successful, otherwise false</returns>
        Task<bool> DeleteAsync(int id);
    }
}
