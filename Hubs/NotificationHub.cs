using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace src.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        // agrega usuario al canal 
        public override async Task OnConnectedAsync()
        {
            var UserId = GetCurrentUserId();
            _logger.LogInformation($"user: {UserId}");

            if (!string.IsNullOrEmpty(UserId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{UserId}");
                await Clients.Caller.SendAsync("SubscriptionConfirmed", UserId);
            }

            await base.OnConnectedAsync();
        }

        // desconecta al usuario del canal 
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = GetCurrentUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                _logger.LogInformation($"Usuario {userId} desconectado");
            }

            await base.OnDisconnectedAsync(exception);
        }


        public async Task SubscribeToNotification()
        {
            var userid = GetCurrentUserId();
            if (!string.IsNullOrEmpty(userid))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userid}");
                await Clients.Caller.SendAsync("SubscriptionConfirmed", userid);

                _logger.LogInformation($"Usuario suscrito a notifications: {userid}");
            }
        }

        // marca la notificacion como leida
        public async Task MarkAsRead(string notificationId)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation($"Usuario {userId} marco como leida: {notificationId}");
        }

        // obtiene id de usuario de los claim  
        private string? GetCurrentUserId()
        {
            var user = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(user))
            {
                user = Context.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            }
            System.Console.WriteLine(user);
            return user;
        }

        public async Task DebugUserInfo()
        {
            var userInfo = new
            {
                ConnectionId = Context.ConnectionId,
                IsAuthenticated = Context.User?.Identity?.IsAuthenticated ?? false,
                UserIdentityName = Context.User?.Identity?.Name,
                NameIdentifier = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Sub = Context.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value,
                AllClaims = Context.User?.Claims.Select(c => new { c.Type, c.Value }).ToList()
            };

            _logger.LogInformation("Debug UserInfo: {@UserInfo}", userInfo);
            await Clients.Caller.SendAsync("ReceiveDebugInfo", userInfo);
        }
    }
}