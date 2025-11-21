using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Interfaces;
using BackEndNotes.Utils;
using BlogNotasBackend.Interfaces.Principals;
using BlogNotasBackend.requests;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using src.Hubs;
using src.Models;

namespace src.Collections
{
    public class NotificationsCollection : INotificationService<NotificationRequest, NotificationModel>
    {
        private readonly IMongoCollection<NotificationModel> _collection;
        private readonly ILogger<NotificationsCollection> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly int cantidad = 20;

        public NotificationsCollection(Context context, ILogger<NotificationsCollection> logger, IHubContext<NotificationHub> hubContext)
        {
            _collection = context.GetCollection<NotificationModel>("Notifications");
            _hubContext = hubContext;
            _logger = logger;
        }

        // se remueve la 
        public async Task<bool> Remove(string id)
        {
            var filter = Builders<NotificationModel>.Filter.Eq(x => x.Id, Change(id));
            var data = await _collection.DeleteOneAsync(filter);
            if (data.IsAcknowledged) return true;
            return false;
        }

        // enviar notificacion a varios usuarios
        public async Task SendMultiplesUserAsync(List<string> ids, NotificationModel notificacion)
        {
            var tasks = ids.Select(x => SendToUserAsync(x, notificacion));
            await Task.WhenAll(tasks);
        }

        //  envia envia notificacion
        public async Task SendNotificationAsync(NotificationRequest request)
        {
            var data = new NotificationModel
            {
                Title = request.Title,
                Message = request.Message,
                IsRead = false,
                TargetUserId = Change(request.UserRefId),
                TargetUserName = request.UserRefName,
                AtCreated = DateTime.UtcNow,
            };

            await _collection.InsertOneAsync(data);
            await SendToUserAsync(request.UserRefId, data);
        }

        // envia una notificacion a un usuario 
        public async Task SendToUserAsync(string id, NotificationModel Notification)
        {
            try
            {
                await _hubContext.Clients.Group($"user-{Notification.TargetUserId}").SendAsync("ReceiveNotification", Notification);
                _logger.LogInformation($"Notification send to user {id}: {Notification.Title}");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"No se pudo enviar la notificacion a: {id} ");
            }
        }

        // actualiza si ya se vio
        public async Task<bool> UpdateData(string id, NotificationModel model)
        {
            var filter = Builders<NotificationModel>.Filter.Eq(x => x.Id, Change(id));
            var update = Builders<NotificationModel>.Update.Set(x => x.IsRead, model.IsRead);

            var GetUpdate = await _collection.UpdateOneAsync(filter, update);
            if (GetUpdate.IsModifiedCountAvailable) return true;
            return false;
        }

        public async Task<List<NotificationModel>> ViewAllDataIdUser(string userId, int pagina)
        {
            var filtro = Builders<NotificationModel>.Filter.Eq(x => x.IdUserReference, Change(userId));
            return await _collection.Find(filtro)
            .Skip((pagina - 1) * cantidad)
            .SortByDescending(x => x.AtCreated)
            .Limit(cantidad)
            .ToListAsync();
        }

        private ObjectId Change(string id)
        {
            return new ObjectId(id);
        }
    }
}