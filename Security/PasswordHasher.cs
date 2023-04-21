using Microsoft.CodeAnalysis.Scripting;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Policy;
using TimeX.Core.Services;

namespace TimeX.Security
{
    public class PasswordHasher: IPasswordHasher
    {

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[37];
            Array.Copy(salt, 0, hashBytes, 1, 16);
            Array.Copy(hash, 0, hashBytes, 17, 20);

            return Convert.ToBase64String(hashBytes);
        }

        public bool PasswordMatches(string providedPassword, string passwordHash)
        {
            if (passwordHash == null)
            {
                return false;
            }
            if (providedPassword == null)
            {
                throw new ArgumentNullException(nameof(providedPassword));
            }
            byte[] src = Convert.FromBase64String(passwordHash);
            byte[] salt = new byte[16];
            byte[] hashPassowrd = new byte[20];

            Array.Copy(src, 1, salt, 0, 16);
            Array.Copy(src, 17, hashPassowrd, 0, 20);

            var pbkdf2 = new Rfc2898DeriveBytes(providedPassword, salt, 10000);
            byte[] Generatedhash = pbkdf2.GetBytes(20);

            return ByteArraysEqual(Generatedhash, hashPassowrd);
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private bool ByteArraysEqual(byte[] a, byte[] b)
        {

            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            bool areSame = true;
            for (int i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }


    }
}
