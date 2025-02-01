using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BackEndNotes.Interfaces;
using BackEndNotes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BackEndNotes.Utils
{
    public class NotificationMail : INotification<MailModel>
    {
        private readonly IConfiguration _configuration;
        public NotificationMail(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool Notificar(MailModel notification)
        {
            if (notification == null) return false; // valida que los datos los esta pasando a la funcion 

            // Obtener datos de entrada del archivo appsettings
            var Host = _configuration.GetSection("Email:Host").Value;
            var User = _configuration.GetSection("Email:UserName").Value;
            var Port = Convert.ToInt32(_configuration.GetSection("Email:Port").Value);
            var Password = _configuration.GetSection("Email:Password").Value;

            // Configuración del cliente SMTP
            SmtpClient smtpClient = new SmtpClient(Host, Port)
            {
                Credentials = new NetworkCredential(User, Password),
                EnableSsl = true
            };
            // Crear el mensaje de correo
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(User),
                Subject = notification.Motivo,
                Body = notification.Message,
                IsBodyHtml = true // Si quieres que el correo soporte HTML, pon esto a true
            };
            // Destinatario del correo
            mail.To.Add(notification.Mail);
            // Enviar el correo
            smtpClient.Send(mail);
            return true;

        }
    }
}