using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PermissionSystem.Contracts.DTOs.Auth;

namespace PermissionSystem.Application.Services.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Logs in a user and returns an authentication response.
        /// </summary>
        /// <param name="username"> Username of the user.</param>
        /// <param name="password"> Password of the user.</param>
        /// <returns></returns>
        Task<AuthResponseDto> LoginAsync(string username, string password);
    }
}
