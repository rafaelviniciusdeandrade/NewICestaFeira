

namespace CestaFeira.Domain.Interfaces.CryptographyService
{
    public interface ICryptographyService
    {
        string Encrypt(string value);
        string Decrypt(string value);
        int RandomInteger(int min, int max);
        string EncryptToString(string plain_text, string password, string salt);
        string DecryptFromString(string encrypted_bytes_string, string password, string salt);
        string RandomSalt();
    }
}
