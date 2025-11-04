using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionSystem.Application.Services.Interfaces
{
    public interface INotificationSRService
    {
        /// <summary>
        /// Notifies a user with a message. 
        /// </summary>
        /// <param name="userId"> User Id</param>  
        /// <param name="message"> Message to send</param>
        /// <returns> A task representing the asynchronous operation.</returns>
        Task NotifyUserAsync(int userId, string message);

        /// <summary>
        /// Notifies all users in a hierarchy with a message.
        /// </summary>
        /// <param name="hierarchyId"> Hierarchy Id</param>
        /// <param name="message"> Message to send</param>
        /// <returns> A task representing the asynchronous operation.</returns>
        Task NotifyHierarchyAsync(int hierarchyId, string message);
    }
}
