using CestaFeira.Web.Models.Pix;

namespace CestaFeira.Web.Services.Pix
{
    public interface IPixService
    {
        Task<PixResponse> GerarQrCodePix(decimal valor, string cpfOuCnpjProdutor);
    }
}
