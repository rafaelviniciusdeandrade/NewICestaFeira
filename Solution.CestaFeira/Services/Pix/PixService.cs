using CestaFeira.Web.Models.Pix;
using QRCoder;
using System.Drawing;

namespace CestaFeira.Web.Services.Pix
{
    public class PixService : IPixService
    {
        public async Task<PixResponse> GerarQrCodePix(decimal valor, string cpfOuCnpjProdutor)
        {
            string pixPayload = ObterPayloadPixSimulado(valor, cpfOuCnpjProdutor);

            if (string.IsNullOrEmpty(pixPayload))
            {
                return new PixResponse { Success = false, Message = "Payload Pix (Copia e Cola) não foi gerado corretamente." };
            }

            try
            {
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(pixPayload, QRCodeGenerator.ECCLevel.Q);

                    // 🛑 NOVO MÉTODO: Usa PngByteQRCode para retornar um array de bytes (mais estável que Base64QRCode)
                    using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                    {
                        // CORRETO: O método para obter o array de bytes da imagem PNG é 'GetGraphic'.
                        byte[] qrCodeBytes = qrCode.GetGraphic(
                            pixelsPerModule: 20
                        // Você pode omitir os argumentos opcionais se quiser usar os valores padrão,
                        // mas 'pixelsPerModule' ajuda a controlar o tamanho.
                        );

                        // Converte o array de bytes para Base64
                        string qrCodeImageAsBase64 = Convert.ToBase64String(qrCodeBytes);

                        return new PixResponse
                        {
                            Success = true,
                            QrCodeBase64 = qrCodeImageAsBase64,
                            PixCode = pixPayload,
                            Message = "QR Code gerado com sucesso." // Mantenha esta mensagem de sucesso
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                // Se o erro ainda for aqui, a mensagem será capturada pelo JS.
                return new PixResponse { Success = false, Message = $"Erro interno na geração do QR Code: {ex.Message}" };
            }
        }

        // ... (ObterPayloadPixSimulado continua aqui) ...
        private string ObterPayloadPixSimulado(decimal valor, string cpfOuCnpjProdutor)
        {
            // ... (Seu código de simulação do Payload Pix) ...
            string txId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);

            return $"00020126580014BR.GOV.BCB.PIX01{cpfOuCnpjProdutor.Length:D2}{cpfOuCnpjProdutor}52040000530398654{valor:F2}5802BR5913CLIENTE TESTE6008CIDADE SP62150511{txId}6304XXXX";
        }
    }
}