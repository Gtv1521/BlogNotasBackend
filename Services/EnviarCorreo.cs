using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Notas_Back.Services
{
    public class EnviarCorreo
    {
        private readonly IConfiguration _config;
        public EnviarCorreo(IConfiguration configuration)
        {
            _config = configuration;
        }

        public bool EnviarCorreoNotificacion(string destino, string Message)
        {
            try
            {
                var Host = _config.GetSection("Email:Host").Value;
                var User = _config.GetSection("Email:UserName").Value;
                var Port = Convert.ToInt32(_config.GetSection("Email:Port").Value);
                var Password = _config.GetSection("Email:Password").Value;
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
                    Subject = "Notification de Bienvenida MyBloggg",
                    Body = Message,
                    IsBodyHtml = true // Si quieres que el correo soporte HTML, pon esto a true
                };

                // Destinatario del correo
                mail.To.Add(destino);

                // Enviar el correo
                smtpClient.Send(mail);
                return true;
                // Console.WriteLine("Correo enviado con éxito.");
            }
            catch (Exception ex)
            {   
                return false;
                // Console.WriteLine($"Error al enviar el correo: {ex.Message}");
            }
        }
    }
}