using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FMBusiness.Managers
{
    public static class SecurityManager
    {

        private static byte[] _byteSalt { get; set; }

        private static byte[] GenerateSalt(int length)
        {
            var salt = new byte[length];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            _byteSalt = salt;
            return salt;
        }

        // salt getter to return generated salt to db next to the users password
        public static byte[] GetSalt()
        {
            return _byteSalt;
        }

        public static string CalculateHash(string data, byte[] salt = null)
        {
            using (var algorithm = SHA512.Create())
            {
                if (salt == null)
                {
                    salt = GenerateSalt(16);
                }

                var bytes = KeyDerivation.Pbkdf2(data, salt, KeyDerivationPrf.HMACSHA512, 1000, 16);

                return $"{ Convert.ToBase64String(salt) }:{ Convert.ToBase64String(bytes) }";
            }
        }

        public static bool ComparePasswords(string hash, string data)
        {
            try
            {

                /*var parts = hash.Split(':');

                var salt = Convert.FromBase64String(parts[0]);

                var bytes = KeyDerivation.Pbkdf2(data, salt, KeyDerivationPrf.HMACSHA512, 1000, 16);

                string pass = Convert.ToBase64String(bytes);

                return parts[1] == pass;*/
                return hash.Equals(data);

            } catch
            {

                return false;

            }
        }

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "FINANCES_MANAGER_BY_BR";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "FINANCES_MANAGER_BY_BR";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }


    }
}
