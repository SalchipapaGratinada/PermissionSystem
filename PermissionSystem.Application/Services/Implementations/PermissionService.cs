using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PermissionSystem.Application.Services.Interfaces;
using PermissionSystem.Contracts.DTOs.Permissions;
using PermissionSystem.Domain.Entities;
using PermissionSystem.Infrastructure.Data;

namespace PermissionSystem.Application.Services.Implementations
{
    public class PermissionService : IPermissionService
    {
        /// <summary>
        /// The application database context.
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor for PermissionService.
        /// </summary>
        /// <param name="context"> The application database context.</param>
        public PermissionService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all permissions.
        /// </summary>
        /// <returns> A list of PermissionDto objects.</returns>
        public async Task<IEnumerable<PermissionDto>> GetAllAsync()
        {
            return await _context.Permissions
                .Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Codigo = p.Codigo,
                    Descripcion = p.Descripcion
                })
                .ToListAsync();
        }

        /// <summary>
        /// Gets a permission by its ID.
        /// </summary>
        /// <param name="id"> The ID of the permission.</param>
        /// <returns> A PermissionDto object or null if not found.</returns>
        public async Task<PermissionDto?> GetByIdAsync(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null) return null;

            return new PermissionDto
            {
                Id = permission.Id,
                Codigo = permission.Codigo,
                Descripcion = permission.Descripcion
            };
        }

        /// <summary>
        /// Creates a new permission.
        /// </summary>
        /// <param name="dto"> The PermissionDto object containing permission details.</param>
        /// <returns> The created PermissionDto object.</returns>
        public async Task<PermissionDto> CreateAsync(PermissionDto dto)
        {
            var entity = new Permission
            {
                Codigo = dto.Codigo,
                Descripcion = dto.Descripcion
            };

            _context.Permissions.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }

        /// <summary>
        /// Updates an existing permission.
        /// </summary>
        /// <param name="dto"> The PermissionDto object containing updated permission details.</param>
        /// <returns> True if the update was successful; otherwise, false.</returns>
        public async Task<bool> UpdateAsync(PermissionDto dto)
        {
            var existing = await _context.Permissions.FindAsync(dto.Id);
            if (existing == null) return false;

            existing.Codigo = dto.Codigo;
            existing.Descripcion = dto.Descripcion;

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Deletes a permission by its ID.
        /// </summary>
        /// <param name="id"> The ID of the permission to delete.</param>
        /// <returns> True if the deletion was successful; otherwise, false.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Permissions.FindAsync(id);
            if (entity == null) return false;

            _context.Permissions.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
