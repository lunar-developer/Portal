using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Website.UnitTest
{
    public class Program
    {
        private class PasswordFinder : IPasswordFinder
        {
            private char[] password;

            public PasswordFinder(char[] password)
            {
                this.password = password;
            }

            public char[] GetPassword()
            {
                return (char[])this.password.Clone();
            }
        }

        private static void Main()
        {
            try
            {
                string privateKey = string.Empty;
                string password = "";
                string input = string.Empty;
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
                AsymmetricCipherKeyPair asymmetricCipherKeyPair =
                    (password != null)
                        ? ((AsymmetricCipherKeyPair) new PemReader(new System.IO.StringReader(privateKey),
                            new PasswordFinder(password.ToCharArray())).ReadObject())
                        : ((AsymmetricCipherKeyPair) new PemReader(new System.IO.StringReader(privateKey))
                            .ReadObject());

                RsaKeyPairGenerator rsaKeyPairGenerator = new RsaKeyPairGenerator();

                AsymmetricKeyParameter asymmetricKeyParameter = (AsymmetricKeyParameter)new PemReader(new System.IO.StringReader(privateKey)).ReadObject();
                Pkcs1Encoding pkcs1Encoding = new Pkcs1Encoding(new RsaEngine());
                pkcs1Encoding.Init(true, asymmetricKeyParameter);
                
                ISigner signer = SignerUtilities.GetSigner("SHA1withRSA");
                signer.Init(true, asymmetricCipherKeyPair.Private);
                signer.BlockUpdate(bytes, 0, bytes.Length);
                byte[] inArray = signer.GenerateSignature();
                string result = System.Convert.ToBase64String(inArray);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            Console.ReadLine();
        }
    }
}