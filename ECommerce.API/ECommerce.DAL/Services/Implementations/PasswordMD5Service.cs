using ECommerce.DAL.Data;
using ECommerce.DAL.DTO;
using ECommerce.DAL.DTO.User.DataIn;
using ECommerce.DAL.Services.Interfaces;
using ECommerce.DAL.UOWs;
using ECommerce.DAL.Models;
using ECommerce.DAL.DTO.User.DataOut;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.IdentityModel.Tokens;
using ECommerce.DAL.DTO.Product.DataOut;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.DAL.Services.Implementations
{
    public class PasswordMD5Service : IPasswordMD5Service
    {
        public string GetMd5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }
        public bool VerifyMd5Hash(string input, string hash)
        {
            string hashOfInput = GetMd5Hash(input);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
                return true;

            return false;
        }
    }
}
