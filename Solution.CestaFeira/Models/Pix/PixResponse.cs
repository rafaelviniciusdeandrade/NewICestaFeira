namespace CestaFeira.Web.Models.Pix
{
    public class PixResponse
    {
        public bool Success { get; set; }
        public string QrCodeBase64 { get; set; } 
        public string PixCode { get; set; }
        public string Message { get; set; }
    }
}
