using System;
using System.Security.Cryptography;
using System.Text;

namespace ServiceCore.Services.HashService
{
    public class HashByPbkdf2Sha256 : IHashProvider
    {
        private const int ITERATIONS = 42;

        /// <inheritdoc />
        public string GetTextHash(string textToHash)
        {
            throw new Exception("Невозможно воспользоваться данным методом вычисления хэша без использования соли");
        }

        /// <inheritdoc />
        public string GetTextHash(string textToHash, string salt)
        {
            if (string.IsNullOrEmpty(salt))
                throw new Exception("Невозможно воспользоваться данным методом вычисления хэша без использования соли");

            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            if (saltBytes.Length < 8)
                throw new Exception("Длина соли должна быть минимум 8 байт");

            byte[] hashBytes;
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                textToHash,
                saltBytes,
                ITERATIONS,
                HashAlgorithmName.SHA256))
            {
                hashBytes = pbkdf2.GetBytes(32);
            }

            var textHash = Convert.ToBase64String(hashBytes);
            return textHash;
        }
    }
}