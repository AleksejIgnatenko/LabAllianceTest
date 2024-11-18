using LabAllianceTest.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace LabAllianceTest.Helpers
{
    public class PasswordHasher : IPasswordHasher
    {
        private const string FixedSalt = "fixed_salt";

        // Создание хеша
        public string Generate(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + FixedSalt;
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // Сравнение хеша
        public bool Verify(string password, string hashedPassword)
        {
            return hashedPassword.Equals(password);
        }
    }
}
