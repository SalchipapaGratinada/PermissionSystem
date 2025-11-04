using PermissionSystem.Contracts.DTOs.Notifications;
using PermissionSystem.Domain.Entities;

namespace PermissionSystem.Application.Services.Interfaces
{
    public interface INotificationService
    {
        /// <summary>
        /// Gets all notifications.
        /// </summary>
        /// <returns> A list of NotificationDto objects.</returns>
        Task<IEnumerable<NotificationDto>> GetAllAsync();

        /// <summary>
        /// Gets a notification by its ID.
        /// </summary>
        /// <param name="id"> The ID of the notification.</param>
        /// <returns> A NotificationDto object or null if not found.</returns>
        Task<NotificationDto?> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new notification. 
        /// </summary>
        /// <param name="dto"> The NotificationDto object containing notification details.</param>
        /// <returns> The created NotificationDto object.</returns>
        Task<NotificationDto> CreateAsync(NotificationDto dto);

        /// <summary>
        /// Deletes a notification by its ID.
        /// </summary>
        /// <param name="id"> The ID of the notification.</param>
        /// <returns> True if deleted, false if not found.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Notifies a user with a message.
        /// </summary>
        /// <param name="userId"> User ID to notify.</param>
        /// <param name="message"> Message to send.</param>
        /// <returns> A task representing the asynchronous operation.</returns>
        Task NotifyUserAsync(int userId, string message);

        /// <summary>
        /// Notifies all users in a hierarchy node with a message.
        /// </summary>
        /// <param name="hierarchyNodeId"> Hierarchy node ID.</param>
        /// <param name="message"> Message to send.</param>
        /// <returns> A task representing the asynchronous operation.</returns>
        Task NotifyHierarchyAsync(int hierarchyNodeId, string message);

        /// <summary>
        /// Gets notifications for a specific user.
        /// </summary>
        /// <param name="userId"> User ID.</param>
        /// <param name="onlyUnread"> If true, only unread notifications are returned.</param> 
        /// <returns> A list of NotificationDto objects.</returns>
        Task<IEnumerable<NotificationDto>> GetByUserAsync(int userId, bool onlyUnread = false);

        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        /// <param name="notificationId"> Notification ID.</param>
        /// <returns> True if successful, false otherwise.</returns>
        Task<bool> MarkAsReadAsync(int notificationId);

        /// <summary>
        /// Marks all notifications for a user as read.
        /// </summary>
        /// <param name="userId"> User ID.</param>
        /// <returns> The number of notifications marked as read.</returns>
        Task<int> MarkAllAsReadAsync(int userId);
    }
}
