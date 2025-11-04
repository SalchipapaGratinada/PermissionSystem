using Microsoft.AspNetCore.SignalR;
using PermissionSystem.Domain.Entities;
using PermissionSystem.Infrastructure.Data;
using PermissionSystem.Application.Services.Interfaces;
using PermissionSystem.RealTime.Hubs;

namespace PermissionSystem.Application.Services.Implementations
{
    public class SignalRNotificationService : INotificationSRService
    {
        /// <summary>
        /// The application database context.
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// The SignalR hub context.
        /// </summary>
        private readonly IHubContext<NotificationHub> _hubContext;

        /// <summary>
        /// Constructor for SignalRNotificationService.
        /// </summary>
        /// <param name="context"> The application database context.</param>
        /// <param name="hubContext"> The SignalR hub context.</param>
        public SignalRNotificationService(AppDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Notifies a user with a message.
        /// </summary>
        /// <param name="userId"> The ID of the user to notify.</param>
        /// <param name="message">The notification message.</param>
        /// <returns> A task representing the asynchronous operation.</returns>
        public async Task NotifyUserAsync(int userId, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", message);
        }

        /// <summary>
        /// Notifies all users in a hierarchy with a message.
        /// </summary>
        /// <param name="hierarchyId"> The ID of the hierarchy.</param>
        /// <param name="message"> The notification message.</param>
        /// <returns> A task representing the asynchronous operation.</returns>
        public async Task NotifyHierarchyAsync(int hierarchyId, string message)
        {
            var notification = new Notification
            {
                HierarchyNodeId = hierarchyId,
                Message = message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}
