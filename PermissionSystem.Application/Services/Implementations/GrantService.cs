using Microsoft.EntityFrameworkCore;
using PermissionSystem.Application.Services.Interfaces;
using PermissionSystem.Contracts.DTOs.Grants;
using PermissionSystem.Contracts.DTOs.Permissions;
using PermissionSystem.Domain.Entities;
using PermissionSystem.Infrastructure.Data;

namespace PermissionSystem.Application.Services.Implementations
{
    public class GrantService : IGrantService
    {
        /// <summary>
        /// The application database context.
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// The notification service.
        /// </summary>
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Constructor for GrantService.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public GrantService(AppDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        /// <summary>
        /// Gets all grants.
        /// </summary>
        /// <returns> A list of GrantDto objects.</returns>
        public async Task<IEnumerable<GrantDto>> GetAllAsync()
        {
            var grants = await _context.Grants
                .Include(g => g.Permission)
                .Include(g => g.User)
                .Include(g => g.HierarchyNode)
                .ToListAsync();

            return grants.Select(MapToDto);
        }

        /// <summary>
        /// Gets a grant by its ID.
        /// </summary>
        /// <param name="id"> The ID of the grant.</param>
        /// <returns> A GrantDto object or null if not found.</returns>
        public async Task<GrantDto?> GetByIdAsync(int id)
        {
            var grant = await _context.Grants
                .Include(g => g.Permission)
                .Include(g => g.User)
                .Include(g => g.HierarchyNode)
                .FirstOrDefaultAsync(g => g.Id == id);

            return grant == null ? null : MapToDto(grant);
        }

        /// <summary>
        /// Creates a new grant.
        /// </summary>
        /// <param name="dto"> The GrantDto object containing grant details.</param>
        /// <returns> The created GrantDto object.</returns>
        public async Task<GrantDto> CreateAsync(GrantDto dto)
        {
            var grant = new Grant
            {
                PermissionId = dto.PermissionId,
                UserId = dto.UserId,
                HierarchyNodeId = dto.HierarchyNodeId
            };

            _context.Grants.Add(grant);
            await _context.SaveChangesAsync();

            var permission = await _context.Permissions.FindAsync(grant.PermissionId);
            var message = $"Se te ha asignado el permiso '{permission?.Descripcion ?? "Desconocido"}'.";

            if (grant.UserId != null)
                await _notificationService.NotifyUserAsync(grant.UserId.Value, message);
            else if (grant.HierarchyNodeId != null)
                await _notificationService.NotifyHierarchyAsync(grant.HierarchyNodeId.Value, message);

            grant = await _context.Grants
                .Include(g => g.Permission)
                .Include(g => g.User)
                .Include(g => g.HierarchyNode)
                .FirstAsync(g => g.Id == grant.Id);

            return MapToDto(grant);
        }

        /// <summary>
        /// Updates an existing grant.
        /// </summary>
        /// <param name="dto"> The GrantDto object containing updated grant details.</param>
        /// <returns> True if the update was successful, false otherwise.</returns>
        public async Task<bool> UpdateAsync(GrantDto dto)
        {
            var existing = await _context.Grants.FindAsync(dto.Id);
            if (existing == null) return false;

            existing.PermissionId = dto.PermissionId;
            existing.UserId = dto.UserId;
            existing.HierarchyNodeId = dto.HierarchyNodeId;

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Deletes a grant by its ID.
        /// </summary>
        /// <param name="id"> The ID of the grant to delete.</param>
        /// <returns> True if the deletion was successful, false otherwise.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var grant = await _context.Grants.FindAsync(id);
            if (grant == null) return false;

            _context.Grants.Remove(grant);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets permissions assigned to a specific user.
        /// </summary>
        /// <param name="userId"> The ID of the user.</param>
        /// <returns> A list of PermissionDto objects.</returns>
        public async Task<IEnumerable<PermissionDto>> GetPermissionsByUserAsync(int userId)
        {
            var grants = await _context.Grants
                .Include(g => g.Permission)
                .Where(g => g.UserId == userId)
                .ToListAsync();

            return grants
                .Select(g => new PermissionDto
                {
                    Id = g.Permission.Id,
                    Codigo = g.Permission.Codigo,
                    Descripcion = g.Permission.Descripcion
                });
        }

        /// <summary>
        /// Gets permissions assigned to a specific hierarchy node.
        /// </summary>
        /// <param name="hierarchyNodeId"> The ID of the hierarchy node.</param>
        /// <returns> A list of PermissionDto objects.</returns>
        public async Task<IEnumerable<PermissionDto>> GetPermissionsByHierarchyAsync(int hierarchyNodeId)
        {
            var grants = await _context.Grants
                .Include(g => g.Permission)
                .Where(g => g.HierarchyNodeId == hierarchyNodeId)
                .ToListAsync();

            return grants
                .Select(g => new PermissionDto
                {
                    Id = g.Permission.Id,
                    Codigo = g.Permission.Codigo,
                    Descripcion = g.Permission.Descripcion
                });
        }

        /// <summary>
        /// Maps a Grant entity to a GrantDto.
        /// </summary>
        /// <param name="grant"> The Grant entity.</param>
        /// <returns> The mapped GrantDto.</returns>
        private static GrantDto MapToDto(Grant grant)
        {
            return new GrantDto
            {
                Id = grant.Id,
                PermissionId = grant.PermissionId,
                PermissionCode = grant.Permission?.Codigo ?? string.Empty,
                PermissionDescripcion = grant.Permission?.Descripcion ?? string.Empty,
                UserId = grant.UserId,
                Username = grant.User != null ? $"{grant.User.Nombre} {grant.User.Apellido}" : null,
                HierarchyNodeId = grant.HierarchyNodeId,
                HierarchyNodeName = grant.HierarchyNode?.Name
            };
        }
    }
}
