using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace PermissionSystem.Application.Services.Interfaces
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        /// <summary>
        /// Gets the user ID from the Hub connection context.
        /// </summary>
        /// <param name="connection"> The Hub connection context.</param>
        /// <returns> The user ID as a string.</returns>
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
