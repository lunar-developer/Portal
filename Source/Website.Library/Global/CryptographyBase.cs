using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Website.Library.Enum;

namespace Website.Library.Global
{
    public static class CryptographyBase
    {
        internal static class SymmetricProviderEnum
        {
            public static readonly Type TripleDES = typeof(TripleDESCryptoServiceProvider);
            public static readonly Type Aes = typeof(AesCryptoServiceProvider);
        }

        public static class SymmetricAlgorithmEnum
        {
            public const string TripleDES = "daa132bf11703d8ba0efe2f55a68a91d";
            public const string Aes = "d23347dedc313687b1e346dd0b93acfd";
        }


        internal static class HashProviderEnum
        {
            public static readonly Type MD5 = typeof(MD5CryptoServiceProvider);
            public static readonly Type SHA1 = typeof(SHA1CryptoServiceProvider);
            public static readonly Type SHA256 = typeof(SHA256CryptoServiceProvider);
            public static readonly Type SHA384 = typeof(SHA384CryptoServiceProvider);
            public static readonly Type SHA512 = typeof(SHA512CryptoServiceProvider);
        }

        public static class HashAlgorithmEnum
        {
            public const string MD5 = "d2548bf2801a36af88001f11fbf54361";
            public const string SHA1 = "fc13a7d5e2b337bab8077fa6238284d5";
            public const string SHA256 = "5f94830f94ed3788abad298e40367b5b";
            public const string SHA384 = "9d51bac9a8603b508a604cb3efcd6379";
            public const string SHA512 = "3a882ac7bc633525b777c89c0d9c2d05";
        }


        public static class TripleDESKeySizeEnum
        {
            public const int Length128 = 128;
            public const int Length192 = 192;
        }

        public static class AesKeySizeEnum
        {
            public const int Length128 = 128;
            public const int Length192 = 192;
            public const int Length256 = 256;
        }


        private static readonly Dictionary<string, Type> SymmetricDictionary = new Dictionary<string, Type>();
        private static readonly Dictionary<string, Type> HashDictionary = new Dictionary<string, Type>();


        static CryptographyBase()
        {
            foreach (FieldInfo field in typeof(SymmetricProviderEnum).GetFields())
            {
                Type type = (Type)field.GetValue(null);
                SymmetricDictionary.Add(GetClassGuid(type), type);
            }
            foreach (FieldInfo field in typeof(HashProviderEnum).GetFields())
            {
                Type type = (Type)field.GetValue(null);
                HashDictionary.Add(GetClassGuid(type), type);
            }
        }


        private static string GetClassGuid(Type type)
        {
            return type.GUID.ToString(PatternEnum.GuidDigits);
        }

        public static string GenerateSymmetricKey(
            string algorithm = SymmetricAlgorithmEnum.Aes,
            int keySize = AesKeySizeEnum.Length128)
        {
            if (SymmetricDictionary.ContainsKey(algorithm) == false)
            {
                return string.Empty;
            }

            using (SymmetricAlgorithm provider =
                (SymmetricAlgorithm)Activator.CreateInstance(SymmetricDictionary[algorithm]))
            {
                provider.KeySize = keySize;
                provider.GenerateKey();

                return Convert.ToBase64String(provider.Key);
            }
        }

        public static string EncryptSymmetric(string key, string plainText,
            string algorithm = SymmetricAlgorithmEnum.Aes,
            CipherMode cipherMode = CipherMode.ECB,
            PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            if (SymmetricDictionary.ContainsKey(algorithm) == false)
            {
                return string.Empty;
            }

            using (SymmetricAlgorithm provider =
                (SymmetricAlgorithm)Activator.CreateInstance(SymmetricDictionary[algorithm]))
            {
                provider.Key = Convert.FromBase64String(key);
                provider.Mode = cipherMode;
                provider.Padding = paddingMode;

                byte[] bytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(provider.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
            }
        }

        public static string DecryptSymmetric(string key, string cipherText,
            string algorithm = SymmetricAlgorithmEnum.Aes,
            CipherMode cipherMode = CipherMode.ECB,
            PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            if (SymmetricDictionary.ContainsKey(algorithm) == false)
            {
                return string.Empty;
            }

            using (SymmetricAlgorithm provider =
                (SymmetricAlgorithm)Activator.CreateInstance(SymmetricDictionary[algorithm]))
            {
                provider.Key = Convert.FromBase64String(key);
                provider.Mode = cipherMode;
                provider.Padding = paddingMode;

                byte[] bytes = Convert.FromBase64String(cipherText);
                return Encoding.UTF8.GetString(provider.CreateDecryptor().TransformFinalBlock(bytes, 0, bytes.Length));
            }
        }


        public static string Hash(string plainText,
            string algorithm = HashAlgorithmEnum.SHA256,
            string salt = "")
        {
            if (HashDictionary.ContainsKey(algorithm) == false)
            {
                return string.Empty;
            }

            using (HashAlgorithm provider = (HashAlgorithm)Activator.CreateInstance(HashDictionary[algorithm]))
            {
                byte[] bytes = Encoding.Unicode.GetBytes(plainText + salt);
                bytes = provider.ComputeHash(bytes);
                return bytes.Aggregate(string.Empty, (current, x) => current + $"{x:x2}");
            }
        }
    }
}