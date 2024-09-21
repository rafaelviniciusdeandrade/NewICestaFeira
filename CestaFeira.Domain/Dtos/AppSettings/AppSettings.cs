

namespace CestaFeira.Domain.Dtos.AppSettings
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string CryptografySalt { get; set; }
        public int TokenExpirationHours { get; set; }
        public int RefreshTokenExpirationHours { get; set; }
    }
}
