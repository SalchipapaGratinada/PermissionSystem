using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PermissionSystem.Application.Services.Interfaces;
using PermissionSystem.Contracts.DTOs.Users;
using PermissionSystem.Domain.Entities;
using PermissionSystem.Infrastructure.Data;

namespace PermissionSystem.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        /// <summary>
        /// The application database context.
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor for UserService.
        /// </summary>
        /// <param name="context"> The application database context.</param>
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns> A list of UserDto objects.</returns>
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.HierarchyNode)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Nombre,
                    LastName = u.Apellido,
                    Blood = u.TipoSangre,
                    HierarchyNodeId = u.HierarchyNodeId,
                    HierarchyNodeName = u.HierarchyNode != null ? u.HierarchyNode.Name : null
                })
                .ToListAsync();
        }

        /// <summary>
        /// Gets a user by its ID.
        /// </summary>
        /// <param name="id"> The ID of the user.</param>
        /// <returns> A UserDto object or null if not found.</returns>
        public async Task<UserDto?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.HierarchyNode)
                .Where(u => u.Id == id)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Nombre,
                    LastName = u.Apellido,
                    Blood = u.TipoSangre,
                    HierarchyNodeId = u.HierarchyNodeId,
                    HierarchyNodeName = u.HierarchyNode != null ? u.HierarchyNode.Name : null
                })
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user"> The User entity containing user details.</param>
        /// <returns> The created UserDto object.</returns>
        public async Task<UserDto> CreateAsync(User user)
        {
            user.Username = $"{user.Nombre.ToLower()}.{user.Apellido.ToLower()}";
            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, "admin12345*");

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var hierarchy = await _context.HierarchyNodes.FindAsync(user.HierarchyNodeId);

            return new UserDto
            {
                Id = user.Id,
                Username = user.Nombre,
                LastName = user.Apellido,
                Blood = user.TipoSangre,
                HierarchyNodeId = user.HierarchyNodeId,
                HierarchyNodeName = hierarchy?.Name
            };
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="user"> The User entity containing updated user details.</param>
        /// <returns> The updated UserDto object.</returns>
        public async Task<UserDto> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var hierarchy = await _context.HierarchyNodes.FindAsync(user.HierarchyNodeId);

            return new UserDto
            {
                Id = user.Id,
                Username = user.Nombre,
                LastName = user.Apellido,
                Blood = user.TipoSangre,
                HierarchyNodeId = user.HierarchyNodeId,
                HierarchyNodeName = hierarchy?.Name
            };
        }

        /// <summary>
        /// Deletes a user by its ID.
        /// </summary>
        /// <param name="id"> The ID of the user.</param>
        /// <returns> True if the user was deleted, false otherwise.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
