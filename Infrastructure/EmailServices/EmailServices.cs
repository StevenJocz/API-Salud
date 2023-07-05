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

namespace UNAC.AppSalud.Infrastructure.EmailServices
{
    public interface IEmailServices
    {
        Task<EmailRespose> EmailRestablecimientoPassword(EmailDTOs request);
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

        private bool EnviarEmail(int Accion, EmailDTOs request, string nombre, string codigo)
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
            var Email = _context.LoginEs.FirstOrDefault(x => x.userEmail == request.Para);

            if (Email == null)
            {
                return new EmailRespose
                {
                    resultado = false,
                    message = "No existe un usuario asociado al correo " + request.Para,
                    codigo = null
                };
            }
            else
            { 
                int Accion = 1;
                string codigo = GenerarCodigo(6);

                bool Enviado = EnviarEmail(Accion, request, Email.user_name, codigo);

                if (Enviado) {

                    return  new EmailRespose
                    {
                            resultado = true,
                            message = "El correo electrónico se ha enviado correctamente.",
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

       
    }
}
