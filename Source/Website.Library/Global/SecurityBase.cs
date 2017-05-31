using System;
using System.Security.Cryptography;
using System.Text;

namespace Website.Library.Global
{
    public static class SecurityBase
    {
        /*
         *  Properties
         */
        private const int KeySize = 128;


        /*
         *  Functions
         */
        public static string GenerateKey()
        {
            TripleDES provider = new TripleDESCryptoServiceProvider();
            provider.KeySize = KeySize;
            provider.GenerateKey();
            return Convert.ToBase64String(provider.Key);
        }

        public static string Encrypt(string key, string plainText)
        {
            using (TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider())
            {
                provider.Key = Encoding.UTF8.GetBytes(key);
                provider.Mode = CipherMode.ECB;
                provider.Padding = PaddingMode.PKCS7;

                byte[] bytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(provider.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
            }
        }

        public static string Decrypt(string key, string cipherText)
        {
            using (TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider())
            {
                provider.Key = Encoding.UTF8.GetBytes(key);
                provider.Mode = CipherMode.ECB;
                provider.Padding = PaddingMode.PKCS7;

                byte[] bytes = Convert.FromBase64String(cipherText);
                return Encoding.UTF8.GetString(provider.CreateDecryptor().TransformFinalBlock(bytes, 0, bytes.Length));
            }
        }

        public static string ToSHA1String(string password, string salt = null)
        {
            SHA1CryptoServiceProvider provider = new SHA1CryptoServiceProvider();
            StringBuilder buffer = new StringBuilder();
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
            bytes = provider.ComputeHash(bytes);
            foreach (byte b in bytes)
            {
                buffer.Append(b.ToString("X2"));
            }
            return "0x" + buffer;
        }
    }
}