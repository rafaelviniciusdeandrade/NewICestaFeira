using System.Security.Cryptography;
using System.Text;

namespace CestaFeira.Domain.Helpers
{
    public class CryptographyHelper
    {
        private RSACryptoServiceProvider rsa;

        public CryptographyHelper()
        {
            rsa = new RSACryptoServiceProvider();
        }

        public string GenerateSalt()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                string publicKey = rsa.ToXmlString(false);

                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(publicKey));
                    return Convert.ToBase64String(hashBytes);
                }
            }
        }

        public string Encrypt(string texto, string salt)
        {
            using (Aes aesAlg = Aes.Create())
            {
                Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes("SecreTeste", Encoding.UTF8.GetBytes(salt), 10000);
                aesAlg.Key = keyDerivation.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = new byte[aesAlg.BlockSize / 8];

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                byte[] textoBytes = Encoding.UTF8.GetBytes(texto);
                byte[] textoCriptografadoBytes = encryptor.TransformFinalBlock(textoBytes, 0, textoBytes.Length);

                return Convert.ToBase64String(aesAlg.IV) + "." + Convert.ToBase64String(textoCriptografadoBytes) + "." + salt;
            }
        }

        public string Decrypt(string textoCriptografado)
        {
            string[] partes = textoCriptografado.Split('.');

            byte[] iv = Convert.FromBase64String(partes[0]);
            byte[] textoCriptografadoBytes = Convert.FromBase64String(partes[1]);
            string salt = partes[2];

            using (Aes aesAlg = Aes.Create())
            {
                Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes("SecreTeste", Encoding.UTF8.GetBytes(salt), 10000);
                aesAlg.Key = keyDerivation.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                byte[] textoDescriptografadoBytes = decryptor.TransformFinalBlock(textoCriptografadoBytes, 0, textoCriptografadoBytes.Length);

                return Encoding.UTF8.GetString(textoDescriptografadoBytes);
            }
        }

    }
}
