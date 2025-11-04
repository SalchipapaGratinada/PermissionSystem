using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PermissionSystem.Application.Services.Interfaces;
using PermissionSystem.Contracts.DTOs.Notifications;
using PermissionSystem.Domain.Entities;

namespace PermissionSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        /// <summary>
        /// The notification service used for managing notifications.
        /// </summary>
        private readonly INotificationService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationController"/> class.
        /// </summary>
        /// <param name="service"> The notification service.</param>
        public NotificationController(INotificationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all notifications.
        /// </summary>
        /// <returns> A list of all notifications. </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetAll()
            => Ok(await _service.GetAllAsync());

        /// <summary>
        /// Gets a notification by its ID.
        /// </summary>
        /// <param name="id"> The ID of the notification. </param>
        /// <returns> The notification with the specified ID. </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationDto>> GetById(int id)
        {
            var notification = await _service.GetByIdAsync(id);
            return notification == null ? NotFound() : Ok(notification);
        }

        /// <summary>
        /// Gets notifications for a specific user.
        /// </summary>
        /// <param name="userId"> The ID of the user. </param>
        /// <param name="onlyUnread"> Whether to filter only unread notifications. </param>
        /// <returns> A list of notifications for the specified user. </returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetByUser(int userId, [FromQuery] bool onlyUnread = false)
            => Ok(await _service.GetByUserAsync(userId, onlyUnread));

        /// <summary>
        /// Creates a new notification.
        /// </summary>
        /// <param name="dto"> The notification data transfer object. </param>
        /// <returns> The created notification. </returns>
        [HttpPost]
        public async Task<ActionResult<NotificationDto>> Create(NotificationDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Sends a notification to a specific user.
        /// </summary>
        /// <param name="userId"> The ID of the user. </param>
        /// <param name="message"> The notification message. </param>
        /// <returns> A success message. </returns>
        [HttpPost("notify-user/{userId}")]
        public async Task<IActionResult> NotifyUser(int userId, [FromBody] string message)
        {
            await _service.NotifyUserAsync(userId, message);
            return Ok(new { success = true, message = "Notificación enviada al usuario." });
        }

        /// <summary>
        /// Sends a notification to all users in a specific hierarchy node.
        /// </summary>
        /// <param name="hierarchyNodeId"> The ID of the hierarchy node. </param>
        /// <param name="message"> The notification message. </param>
        /// <returns> A success message. </returns>
        [HttpPost("notify-hierarchy/{hierarchyNodeId}")]
        public async Task<IActionResult> NotifyHierarchy(int hierarchyNodeId, [FromBody] string message)
        {
            await _service.NotifyHierarchyAsync(hierarchyNodeId, message);
            return Ok(new { success = true, message = "Notificaciones enviadas a la jerarquía." });
        }

        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        /// <param name="id"> The ID of the notification. </param>
        /// <returns> No content if successful, otherwise not found. </returns>
        [HttpPatch("{id}/mark-read")]
        public async Task<IActionResult> MarkAsRead(int id)
            => await _service.MarkAsReadAsync(id) ? NoContent() : NotFound();

        /// <summary>
        /// Marks all notifications for a user as read.
        /// </summary>
        /// <param name="userId"> The ID of the user. </param>
        /// <returns> The count of notifications marked as read. </returns>
        [HttpPatch("user/{userId}/mark-all-read")]
        public async Task<IActionResult> MarkAllAsRead(int userId)
            => Ok(new { count = await _service.MarkAllAsReadAsync(userId) });

        /// <summary>
        /// Deletes a notification by its ID.
        /// </summary>
        /// <param name="id"> The ID of the notification. </param>
        /// <returns> No content if successful, otherwise not found. </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => await _service.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
