using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Interfaces;
using BackEndNotes.Interfaces.Regulars;

namespace BlogNotasBackend.Interfaces.Principals
{
    public interface INotificationService<T, S>: IViewXId<S>, IRemove, IUpdate<S>
    {
        Task SendNotificationAsync(T request);
        Task SendToUserAsync(string id, S Notification);
        Task SendMultiplesUserAsync(List<string> ids, S notificacion);
    }
}