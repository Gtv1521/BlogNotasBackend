using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNotasBackend.requests
{
    public class NotificationRequest
    {
        public string TargetId { get; set; } // id de nota
        public TypeNote Type { get; set; }
        public string Title { get; set; } // titulo de la notification
        public string Message { get; set; } // mensaja a mostrar
        public string UserRefId { get; set; } // usuario referido
        public string UserRefName { get; set; } // nombre de referido
        public object Data { get; set; } // datos adicionales
    }

    public enum TypeNote
    {
        NotefyUser = '1',
        NotifyMail = '2',
        NotifyCel = '3'

    }
}