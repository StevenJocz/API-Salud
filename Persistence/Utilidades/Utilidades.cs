using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UNAC.AppSalud.Persistence.Utilidades
{
    public interface IUtilidades
    {
        Task<string> HashPassword(string password);
    }


    public class Utilidades : IUtilidades
    {
        public async Task<string> HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Generar el hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convertir el array de bytes a una cadena hexadecimal
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static async Task<bool> VerifyPassword(string enteredPassword, string savedHashedPassword)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Generar el hash de la contraseña ingresada
                byte[] enteredPasswordBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));

                // Convertir el array de bytes a una cadena hexadecimal
                StringBuilder enteredPasswordBuilder = new StringBuilder();
                for (int i = 0; i < enteredPasswordBytes.Length; i++)
                {
                    enteredPasswordBuilder.Append(enteredPasswordBytes[i].ToString("x2"));
                }
                string enteredPasswordHash = enteredPasswordBuilder.ToString();

                // Comparar el hash de la contraseña ingresada con el hash de la contraseña almacenada
                return string.Equals(enteredPasswordHash, savedHashedPassword);
            }
        }
    }
}
