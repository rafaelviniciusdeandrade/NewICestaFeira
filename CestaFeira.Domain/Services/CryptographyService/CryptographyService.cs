using CestaFeira.Domain.Dtos.AppSettings;
using CestaFeira.Domain.Interfaces.CryptographyService;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.Services.CryptographyService
{
    public enum CryptProvider
    {
        Rijndael, RC2, DES, TripleDES
    }

    class CryptographyService : ICryptographyService
    {
        private string _key = string.Empty;
        private CryptProvider _cryptProvider;
        private SymmetricAlgorithm _algorithm;

        private readonly IOptions<AppSettings> settings;

        // The random number provider.
        private RNGCryptoServiceProvider Rand;

        public CryptographyService(IOptions<AppSettings> settings)
        {
            this.settings = settings;
            _algorithm = new RijndaelManaged
            {
                Mode = CipherMode.CBC
            };
            _cryptProvider = CryptProvider.Rijndael;
            Rand = new RNGCryptoServiceProvider();
        }


        private void SetIV()

        {
            switch (_cryptProvider)
            {
                case CryptProvider.Rijndael:
                    _algorithm.IV = new byte[] { 0xf, 0x6f, 0x13, 0x2e, 0x35, 0xc2, 0xcd, 0xf9, 0x5, 0x46, 0x9c, 0xea, 0xa8, 0x4b, 0x73, 0xcc };
                    break;
                default:
                    _algorithm.IV = new byte[] { 0xf, 0x6f, 0x13, 0x2e, 0x35, 0xc2, 0xcd, 0xf9 };
                    break;
            }
        }

        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public CryptographyService(CryptProvider cryptProvider, IOptions<AppSettings> settings)
        {
            this.settings = settings;
            switch (cryptProvider)
            {
                case CryptProvider.Rijndael:
                    _algorithm = new RijndaelManaged();
                    _cryptProvider = CryptProvider.Rijndael;
                    break;
                case CryptProvider.RC2:
                    _algorithm = new RC2CryptoServiceProvider();
                    _cryptProvider = CryptProvider.RC2;
                    break;
                case CryptProvider.DES:
                    _algorithm = new DESCryptoServiceProvider();
                    _cryptProvider = CryptProvider.DES;
                    break;
                case CryptProvider.TripleDES:
                    _algorithm = new TripleDESCryptoServiceProvider();
                    _cryptProvider = CryptProvider.TripleDES;
                    break;
            }
            _algorithm.Mode = CipherMode.CBC;
        }

        public virtual byte[] GetKey()
        {
            string salt = settings.Value.CryptografySalt;
            if (_algorithm.LegalKeySizes.Length > 0)
            {
                int keySize = _key.Length * 8;
                int minSize = _algorithm.LegalKeySizes[0].MinSize;
                int maxSize = _algorithm.LegalKeySizes[0].MaxSize;
                int skipSize = _algorithm.LegalKeySizes[0].SkipSize;
                if (keySize > maxSize)
                {
                    _key = _key.Substring(0, maxSize / 8);
                }
                else if (keySize < maxSize)
                {
                    int validSize = keySize <= minSize ? minSize : keySize - keySize % skipSize + skipSize;
                    if (keySize < validSize)
                    {
                        _key = _key.PadRight(validSize / 8, '*');
                    }
                }
            }
            PasswordDeriveBytes key = new PasswordDeriveBytes(_key, Encoding.ASCII.GetBytes(salt));
            return key.GetBytes(_key.Length);
        }
        public virtual string Encrypt(string value)
        {
            byte[] plainByte = Encoding.UTF8.GetBytes(value);
            byte[] keyByte = GetKey();
            _algorithm.Key = keyByte;
            SetIV();
            ICryptoTransform cryptoTransform = _algorithm.CreateEncryptor();
            MemoryStream _memoryStream = new MemoryStream();
            CryptoStream _cryptoStream = new CryptoStream(_memoryStream, cryptoTransform, CryptoStreamMode.Write);
            _cryptoStream.Write(plainByte, 0, plainByte.Length);
            _cryptoStream.FlushFinalBlock();
            byte[] cryptoByte = _memoryStream.ToArray();
            return Convert.ToBase64String(cryptoByte, 0, cryptoByte.GetLength(0));
        }

        public virtual string Decrypt(string value)
        {
            byte[] cryptoByte = Convert.FromBase64String(value);
            byte[] keyByte = GetKey();
            _algorithm.Key = keyByte;
            SetIV();
            ICryptoTransform cryptoTransform = _algorithm.CreateDecryptor();
            try
            {
                MemoryStream _memoryStream = new MemoryStream(cryptoByte, 0, cryptoByte.Length);
                CryptoStream _cryptoStream = new CryptoStream(_memoryStream, cryptoTransform, CryptoStreamMode.Read);
                StreamReader _streamReader = new StreamReader(_cryptoStream);
                return _streamReader.ReadToEnd();
            }
            catch
            {
                return null;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        // Return a random integer between a min and max value.
        public int RandomInteger(int min, int max)
        {
            uint scale = uint.MaxValue;
            while (scale == uint.MaxValue)
            {
                // Get four random bytes.
                byte[] four_bytes = new byte[4];
                Rand.GetBytes(four_bytes);

                // Convert that into an uint.
                scale = BitConverter.ToUInt32(four_bytes, 0);
            }

            // Add min to the scaled difference between max and min.
            return (int)(min + (max - min) * (scale / (double)uint.MaxValue));
        }

        // Encrypt a string into a hex byte string.
        public string EncryptToString(string plain_text, string password, string salt)
        {
            return BytesToHexString(Encrypt(plain_text, password, salt));
        }

        // Decrypt a string from a hex byte string.
        public string DecryptFromString(string encrypted_bytes_string, string password, string salt)
        {
            return Decrypt(HexStringToBytes(encrypted_bytes_string), password, salt);
        }

        // Encrypt a string into an array of bytes.
        private byte[] Encrypt(string plain_text, string password, string salt)
        {
            ASCIIEncoding ascii_encoder = new ASCIIEncoding();
            byte[] plain_bytes = ascii_encoder.GetBytes(plain_text);
            return CryptBytes(password, plain_bytes, salt, true);
        }

        // Decrypt a string from an array of bytes.
        private string Decrypt(byte[] encrypted_bytes, string password, string salt)
        {
            byte[] decrypted_bytes = CryptBytes(password, encrypted_bytes, salt, false);
            ASCIIEncoding ascii_encoder = new ASCIIEncoding();
            return new string(ascii_encoder.GetChars(decrypted_bytes));
        }

        // Convert bytes into a string.
        private string BytesToHexString(byte[] bytes)
        {
            string result = "";
            foreach (byte b in bytes)
            {
                result += " " + b.ToString("X2");
            }
            if (result.Length > 0) result = result.Substring(1);
            return result;
        }

        // Convert a hex string into bytes.
        private byte[] HexStringToBytes(string str)
        {
            str = str.Replace(" ", "");
            int num_bytes = str.Length / 2;
            byte[] bytes = new byte[num_bytes];
            for (int i = 0; i < num_bytes; i++)
            {
                bytes[i] = Convert.ToByte(str.Substring(2 * i, 2), 16);
            }
            return bytes;
        }

        // Use the password to generate key bytes.
        private void MakeKeyAndIV(string password, byte[] salt, int key_size_bits, int block_size_bits, ref byte[] key, ref byte[] iv)
        {
            Rfc2898DeriveBytes derive_bytes = new Rfc2898DeriveBytes(password, salt, 513);
            key = derive_bytes.GetBytes(key_size_bits / 8);
            iv = derive_bytes.GetBytes(block_size_bits / 8);
        }

        // Encrypt or decrypt a byte array.
        private byte[] CryptBytes(string password, byte[] in_bytes, string salt, bool encrypt)
        {
            // Make an AES service provider.
            AesCryptoServiceProvider aes_provider = new AesCryptoServiceProvider();

            // Find a valid key size for this provider.
            int key_size_bits = 0;
            for (int i = 1024; i >= 1; i--)
            {
                if (aes_provider.ValidKeySize(i))
                {
                    key_size_bits = i;
                    break;
                }
            }

            // Get the block size for this provider.
            int block_size_bits = aes_provider.BlockSize;

            // Convert the salt into bytes.
            byte[] salt_bytes = HexStringToBytes(salt);

            // Generate the key and initialization vector.
            byte[] key = null;
            byte[] iv = null;
            MakeKeyAndIV(password, salt_bytes, key_size_bits, block_size_bits, ref key, ref iv);

            // Make the encryptor or decryptor.
            ICryptoTransform crypto_transform;
            if (encrypt) crypto_transform = aes_provider.CreateEncryptor(key, iv);
            else crypto_transform = aes_provider.CreateDecryptor(key, iv);

            // Create the output stream.
            MemoryStream out_stream = new MemoryStream();

            // Attach a crypto stream to the output stream.
            CryptoStream crypto_stream = new CryptoStream(
               out_stream, crypto_transform,
               CryptoStreamMode.Write);

            // Write the bytes into the CryptoStream.
            crypto_stream.Write(in_bytes, 0, in_bytes.Length);
            try
            {
                crypto_stream.FlushFinalBlock();
            }
            catch (CryptographicException)
            {
                // Ignore this one. The password is bad.
            }
            catch
            {
                // Re-raise this one.
                throw;
            }

            // Save the result.
            byte[] result = out_stream.ToArray();

            // Close the stream.
            try
            {
                crypto_stream.Close();
            }
            catch (CryptographicException)
            {
                // Ignore this one. The password is bad.
            }
            catch
            {
                // Re-raise this one.
                throw;
            }
            out_stream.Close();

            return result;
        }

        // Return a pseudo-random salt.
        public string RandomSalt()
        {
            // Pick a random number of bytes.
            int num_bytes = RandomInteger(10, 20);

            // Fill the salt array.
            byte[] salt_bytes = new byte[num_bytes];
            Rand.GetBytes(salt_bytes);

            // Return the salt converted into a hex string.
            return BytesToHexString(salt_bytes);
        }

    }
}
