using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNotasBackend.Dto.Notifications
{
    public class NotificationsDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Type { get; set; } // "NOTE_SHARED", "NOTE_EDITED", etc.
        public string Title { get; set; }
        public string Message { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string TargetUserId { get; set; }
        public string RelatedEntityId { get; set; } // ID de la nota, etc.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
        public object Data { get; set; } // Datos adicionales
    }
}