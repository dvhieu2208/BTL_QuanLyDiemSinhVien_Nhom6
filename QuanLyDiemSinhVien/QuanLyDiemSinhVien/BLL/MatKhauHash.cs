using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.BLL
{
    internal class MatKhauHash
    {
        private const int Iterations = 600000;
        private const int SaltSize = 16;
        private const int HashSize = 32;
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash;

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(HashSize);
            }

            return "PBKDF2$" + Iterations + "$" +
                   Convert.ToBase64String(salt) + "$" +
                   Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(
            string password,
            string storedHash)
        {
            try
            {
                string[] parts = storedHash.Split('$');

                if (parts.Length != 4 || parts[0] != "PBKDF2")
                    return false;

                int iterations;

                if (!int.TryParse(parts[1], out iterations))
                    return false;

                if (iterations < 100000 || iterations > 2000000)
                    return false;

                byte[] salt = Convert.FromBase64String(parts[2]);
                byte[] expectedHash = Convert.FromBase64String(parts[3]);

                if (salt.Length != SaltSize ||
                    expectedHash.Length != HashSize)
                    return false;

                byte[] actualHash;

                using (var pbkdf2 = new Rfc2898DeriveBytes(
                    password,
                    salt,
                    iterations,
                    HashAlgorithmName.SHA256))
                {
                    actualHash = pbkdf2.GetBytes(HashSize);
                }
                int diff = 0;

                for (int i = 0; i < expectedHash.Length; i++)
                {
                    diff |= expectedHash[i] ^ actualHash[i];
                }

                return diff == 0;
            }
            catch
            {
                return false;
            }
        }
    }
}