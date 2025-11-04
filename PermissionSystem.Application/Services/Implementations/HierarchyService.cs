using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PermissionSystem.Application.Services.Interfaces;
using PermissionSystem.Contracts.DTOs.Hierarchies;
using PermissionSystem.Contracts.DTOs.Users;
using PermissionSystem.Domain.Entities;
using PermissionSystem.Infrastructure.Data;

namespace PermissionSystem.Application.Services.Implementations
{
    public class HierarchyService : IHierarchyService
    {
        /// <summary>
        /// The application database context.
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor for HierarchyService.
        /// </summary>
        /// <param name="context"> The application database context.</param>
        public HierarchyService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all hierarchy nodes.
        /// </summary>
        /// <returns> A list of HierarchyNodeDto objects.</returns>
        public async Task<IEnumerable<HierarchyNodeDto>> GetAllAsync()
        {
            var nodes = await _context.HierarchyNodes
                .Include(h => h.Users)
                .Include(h => h.Parent)
                .ToListAsync();

            return nodes.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Gets a hierarchy node by its ID.
        /// </summary>
        /// <param name="id"> The ID of the hierarchy node.</param>
        /// <returns> A HierarchyNodeDto object or null if not found.</returns>
        public async Task<HierarchyNodeDto?> GetByIdAsync(int id)
        {
            var node = await _context.HierarchyNodes
                .Include(h => h.Users)
                .Include(h => h.Parent)
                .FirstOrDefaultAsync(h => h.Id == id);

            return node != null ? MapToDto(node) : null;
        }

        /// <summary>
        /// Creates a new hierarchy node.
        /// </summary>
        /// <param name="node"> The hierarchy node to create.</param>
        /// <returns> The created HierarchyNodeDto.</returns>
        public async Task<HierarchyNodeDto> CreateAsync(HierarchyNode node)
        {
            _context.HierarchyNodes.Add(node);
            await _context.SaveChangesAsync();

            var created = await _context.HierarchyNodes
                .Include(h => h.Parent)
                .Include(h => h.Users)
                .FirstAsync(h => h.Id == node.Id);

            return MapToDto(created);
        }

        /// <summary>
        /// Updates an existing hierarchy node.
        /// </summary>
        /// <param name="node"> The hierarchy node to update.</param>
        /// <returns> The updated HierarchyNodeDto.</returns>
        public async Task<HierarchyNodeDto> UpdateAsync(HierarchyNode node)
        {
            _context.HierarchyNodes.Update(node);
            await _context.SaveChangesAsync();

            var updated = await _context.HierarchyNodes
                .Include(h => h.Parent)
                .Include(h => h.Users)
                .FirstAsync(h => h.Id == node.Id);

            return MapToDto(updated);
        }

        /// <summary>
        /// Deletes a hierarchy node by its ID.
        /// </summary>
        /// <param name="id"> The ID of the hierarchy node to delete.</param>
        /// <returns> True if deleted, false if not found.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var node = await _context.HierarchyNodes.FindAsync(id);
            if (node == null) return false;

            _context.HierarchyNodes.Remove(node);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets the children of a hierarchy node by its parent ID.
        /// </summary>
        /// <param name="parentId"> The ID of the parent hierarchy node.</param>
        /// <returns> A list of HierarchyNodeDto objects.</returns>
        public async Task<IEnumerable<HierarchyNodeDto>> GetChildrenAsync(int parentId)
        {
            var children = await _context.HierarchyNodes
                .Where(h => h.ParentId == parentId)
                .Include(h => h.Users)
                .Include(h => h.Parent)
                .ToListAsync();

            return children.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Maps a HierarchyNode entity to a HierarchyNodeDto.
        /// </summary>
        /// <param name="node"> The HierarchyNode entity.</param>
        /// <returns> The HierarchyNodeDto.</returns>
        private static HierarchyNodeDto MapToDto(HierarchyNode node)
        {
            return new HierarchyNodeDto
            {
                Id = node.Id,
                Name = node.Name,
                ParentId = node.ParentId,
                ParentName = node.Parent?.Name,
                Users = node.Users.Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Nombre,
                    LastName = u.Apellido,
                    Blood = u.TipoSangre,
                    HierarchyNodeId = u.HierarchyNodeId
                }).ToList()
            };
        }
    }
}
