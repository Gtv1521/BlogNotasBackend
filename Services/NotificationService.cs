using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogNotasBackend.Interfaces.Principals;
using BlogNotasBackend.requests;
using src.Models;

namespace src.Services
{
    public class NotificationService
    {
        private readonly INotificationService<NotificationRequest, NotificationModel> _notify;
        public NotificationService(INotificationService<NotificationRequest, NotificationModel> notify)
        {
            _notify = notify;
        }

        public async Task SendNotify(NotificationRequest notify)
        {
            await _notify.SendNotificationAsync(notify);
        }

        public async Task<IEnumerable<NotificationModel>> GetAll(string userId, int page)
        {
            return await _notify.ViewAllDataIdUser(userId, page);
        }

        public async Task<bool> Update(string id, bool Read)
        {
            return await _notify.UpdateData(id, new NotificationModel { IsRead = Read });
        }
    }
}