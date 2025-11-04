using Microsoft.EntityFrameworkCore;
using PermissionSystem.Application.Services.Interfaces;
using PermissionSystem.Contracts.DTOs.Notifications;
using PermissionSystem.Domain.Entities;
using PermissionSystem.Infrastructure.Data;

namespace PermissionSystem.Application.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        /// <summary>
        /// The application database context.
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// The SignalR notification service.
        /// </summary>
        private readonly INotificationSRService _signalRService;

        /// <summary>
        /// Constructor for NotificationService.
        /// </summary>
        /// <param name="context"> The application database context.</param>
        /// <param name="signalRService"> The SignalR notification service.</param>
        public NotificationService(AppDbContext context, INotificationSRService signalRService)
        {
            _context = context;
            _signalRService = signalRService;
        }

        /// <summary>
        /// Maps a Notification entity to a NotificationDto.
        /// </summary>
        /// <param name="n"> The Notification entity.</param>
        /// <returns> A NotificationDto object.</returns>
        private static NotificationDto MapToDto(Notification n) => new()
        {
            Id = n.Id,
            Message = n.Message,
            CreatedAt = n.CreatedAt,
            IsRead = n.IsRead,
            UserId = n.UserId,
            Username = n.User?.Nombre
        };

        /// <summary>
        /// Maps a NotificationDto to a Notification entity.
        /// </summary>
        /// <param name="dto"> The NotificationDto object.</param>
        /// <returns> A Notification entity.</returns>
        private static Notification MapToEntity(NotificationDto dto) => new()
        {
            Id = dto.Id,
            Message = dto.Message,
            UserId = dto.UserId,
            CreatedAt = dto.CreatedAt == default ? DateTime.UtcNow : dto.CreatedAt,
            IsRead = dto.IsRead
        };

        /// <summary>
        /// Gets all notifications.
        /// </summary>
        /// <returns> A list of NotificationDto objects.</returns>
        public async Task<IEnumerable<NotificationDto>> GetAllAsync()
        {
            var list = await _context.Notifications
                .Include(n => n.User)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return list.Select(MapToDto);
        }

        /// <summary>
        /// Gets a notification by its ID.
        /// </summary>
        /// <param name="id"> The ID of the notification.</param>
        /// <returns> A NotificationDto object or null if not found.</returns>
        public async Task<NotificationDto?> GetByIdAsync(int id)
        {
            var notification = await _context.Notifications
                .Include(n => n.User)
                .FirstOrDefaultAsync(n => n.Id == id);

            return notification == null ? null : MapToDto(notification);
        }

        /// <summary>
        /// Creates a new notification.
        /// </summary>
        /// <param name="dto"> The NotificationDto object containing notification details.</param>
        /// <returns> The created NotificationDto object.</returns>
        public async Task<NotificationDto> CreateAsync(NotificationDto dto)
        {
            var entity = MapToEntity(dto);
            _context.Notifications.Add(entity);
            await _context.SaveChangesAsync();

            var created = await _context.Notifications
                .Include(n => n.User)
                .FirstAsync(n => n.Id == entity.Id);

            return MapToDto(created);
        }

        /// <summary>
        /// Deletes a notification by its ID.
        /// </summary>
        /// <param name="id"> The ID of the notification.</param>
        /// <returns> True if deleted, false if not found.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return false;

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Notifies a user with a message.
        /// </summary>
        /// <param name="userId"> The ID of the user to notify.</param>
        /// <param name="message"> The notification message.</param>
        /// <returns> A task representing the asynchronous operation.</returns>
        public async Task NotifyUserAsync(int userId, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            await _signalRService.NotifyUserAsync(userId, message);
        }

        /// <summary>
        /// Notifies all users in a hierarchy node and its children with a message.
        /// </summary>
        /// <param name="hierarchyNodeId"> The ID of the hierarchy node.</param> 
        /// <param name="message"> The notification message.</param>
        /// <returns> A task representing the asynchronous operation.</returns>
        public async Task NotifyHierarchyAsync(int hierarchyNodeId, string message)
        {
            var users = await _context.Users
                .Where(u => u.HierarchyNodeId == hierarchyNodeId)
                .ToListAsync();

            foreach (var user in users)
                await NotifyUserAsync(user.Id, message);

            var childNodes = await _context.HierarchyNodes
                .Where(h => h.ParentId == hierarchyNodeId)
                .ToListAsync();

            foreach (var child in childNodes)
                await NotifyHierarchyAsync(child.Id, message);
        }

        /// <summary>
        /// Gets notifications for a specific user.
        /// </summary>
        /// <param name="userId"> The ID of the user.</param>
        /// <param name="onlyUnread"> Whether to fetch only unread notifications.</param>
        /// <returns> A list of NotificationDto objects.</returns>
        public async Task<IEnumerable<NotificationDto>> GetByUserAsync(int userId, bool onlyUnread = false)
        {
            IQueryable<Notification> query = _context.Notifications
                .Include(n => n.User)
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt);

            if (onlyUnread)
                query = query.Where(n => !n.IsRead);

            var list = await query.ToListAsync();
            return list.Select(MapToDto);
        }

        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        /// <param name="notificationId"> The ID of the notification.</param>
        /// <returns> True if marked as read, false if not found.</returns>
        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
                return false;

            notification.IsRead = true;
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Marks all notifications for a user as read.
        /// </summary>
        /// <param name="userId"> The ID of the user.</param>
        /// <returns> The number of notifications marked as read.</returns>
        public async Task<int> MarkAllAsReadAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var n in notifications)
                n.IsRead = true;

            await _context.SaveChangesAsync();
            return notifications.Count;
        }
    }
}
