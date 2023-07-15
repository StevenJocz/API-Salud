using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using UNAC.AppSalud.Domain.DTOs.EmailDTOs;
using UNAC.AppSalud.Domain.DTOs.LoginDTOs;
using UNAC.AppSalud.Domain.DTOs.Login.LoginDTOs;
using UNAC.AppSalud.Domain.DTOs.UserDTOs;
using UNAC.AppSalud.Domain.Entities.UserE;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UNAC.AppSalud.Infrastructure.EmailServices
{
    public interface IEmailServices
    {
        Task<EmailRespose> EmailRestablecimientoPassword(EmailDTOs request);
        Task<bool> EmailCreateUser(string correo);
    }

    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration _configuration;
        private readonly SaludDbContext _context = null;

        public EmailServices(IConfiguration configuration)
        {
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection_Salud");
            _context = new SaludDbContext(connectionString);
        }

        private async Task<bool> EnviarEmail(int Accion, EmailDTOs request, string nombre, string codigo)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration.GetSection("Email:UserName").Value));
                email.To.Add(MailboxAddress.Parse(request.Para));
                email.Subject = request.Asunto;
                string contenido = request.Contenido.Replace("[Nombre]", nombre);

                if (Accion == 1)
                {
                    contenido = contenido.Replace("[Código de seguridad]", codigo);
                }

                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = contenido
                };

                using var smtp = new SmtpClient();
                smtp.Connect(
                    _configuration.GetSection("Email:Host").Value,
                    Convert.ToInt32(_configuration.GetSection("Email:Port").Value),
                    SecureSocketOptions.StartTls
                );

                smtp.Authenticate(_configuration.GetSection("Email:UserName").Value, _configuration.GetSection("Email:PassWord").Value);

                smtp.Send(email);
                smtp.Disconnect(true);

                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string GenerarCodigo(int longitud)
        {
            const string caracteres = "0123456789";
            var random = new Random();

            var codigoBuilder = new StringBuilder(longitud);
            for (int i = 0; i < longitud; i++)
            {
                codigoBuilder.Append(caracteres[random.Next(caracteres.Length)]);
            }
            return codigoBuilder.ToString();
        }

        public async Task<EmailRespose> EmailRestablecimientoPassword(EmailDTOs request)
        {
            var Email = _context.UserEs.FirstOrDefault(x => x.s_user_email == request.Para);
            if (Email == null)
            {
                return new EmailRespose
                {
                    resultado = false,
                    message = "No se encontró ningún usuario asociado al correo electrónico proporcionado. Por favor, verifica la dirección de correo electrónico ingresada  ",
                    codigo = null
                };
            }
            else
            { 
                int Accion = 1;
                string codigo = GenerarCodigo(6);
                string NombreCompleto = Email.s_user_name + " " + Email.s_user_lastname;
                bool Enviado = await EnviarEmail(Accion, request, NombreCompleto, codigo);

                if (Enviado) {

                    return  new EmailRespose
                    {
                            resultado = true,
                            message = "Se ha enviado un correo electrónico correctamente.",
                            codigo = codigo
                    };
                }
                else
                {
                    return new EmailRespose
                    {
                        resultado = false,
                        message = "Tenemos problemas al enviar el correo electrónico. Por favor intentalo más tarde.",
                        codigo = null
                    };
                }
                
            }
        }

        public async Task<bool> EmailCreateUser(string correo)
        {

            var request = new EmailDTOs
            {
                Para = correo,
                Asunto = "Bienvenidos a la App de Salud",
                Contenido = @"<!DOCTYPE html><html><head><meta charset=""UTF-8""><title>Bienvenidos a la App de Salud</title><style>body {font-family: Arial, sans-serif;background-color: #f4f4f4;color: #333;}.container {max-width: 600px;margin: 0 auto;padding: 20px;}h1 {color: #555;}p {margin-bottom: 10px;}.footer {margin-top: 30px;font-size: 14px;color: #777;}</style></head><body><div class=""container""><h1>Bienvenido a nuestra app de Salud</h1><p>Hola [Nombre],</p><p>¡Gracias por registrarte en nuestra plataforma! Estamos emocionados de tenerte como parte de nuestra comunidad.</p><p>Estaremos encantados de ayudarte en lo que necesites. No dudes en contactarnos si tienes alguna pregunta o consulta.</p><p>¡Disfruta de todos los beneficios de nuestra plataforma y esperamos que tengas una experiencia increíble!</p><p class=""footer"">Atentamente,<br>El equipo de ejemplo</p></div></body></html>"
            };

            var Email = _context.UserEs.FirstOrDefault(x => x.s_user_email == correo);
            string NombreCompleto = Email.s_user_name + " " + Email.s_user_lastname;
            bool Enviado = await EnviarEmail(2, request, NombreCompleto, "");

            if (Enviado)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
