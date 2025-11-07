using CestaFeira.Web.Models.Pix;
using QRCoder;
using System.Globalization;
using System.Text;

namespace CestaFeira.Web.Services.Pix
{
    public class PixService : IPixService
    {
        private const string ChavePixRecebedor = "07680483620"; // CPF
        private const string NomeRecebedor = "MARLON";
        private const string CidadeRecebedora = "ALFENAS";
        private const string IdentificadorTransacao = "CestaFeira";

        public async Task<PixResponse> GerarQrCodePix(decimal valor, string cpfOuCnpjProdutor)
        {
            string pixPayload = ObterPayloadPix(valor);

            if (string.IsNullOrEmpty(pixPayload))
            {
                return new PixResponse
                {
                    Success = false,
                    Message = "Falha ao gerar o código Pix (payload vazio)."
                };
            }

            try
            {
                using (var qrGenerator = new QRCodeGenerator())
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(pixPayload, QRCodeGenerator.ECCLevel.Q);
                    using (var qrCode = new PngByteQRCode(qrCodeData))
                    {
                        byte[] qrCodeBytes = qrCode.GetGraphic(pixelsPerModule: 20);
                        string qrCodeBase64 = Convert.ToBase64String(qrCodeBytes);

                        return new PixResponse
                        {
                            Success = true,
                            QrCodeBase64 = qrCodeBase64,
                            PixCode = pixPayload,
                            Message = "QR Code gerado com sucesso."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new PixResponse
                {
                    Success = false,
                    Message = $"Erro ao gerar o QR Code Pix: {ex.Message}"
                };
            }
        }

        private string ObterPayloadPix(decimal valor)
        {
            string valorFormatado = valor.ToString("0.00", CultureInfo.InvariantCulture);
            string nome = NomeRecebedor.ToUpperInvariant();
            string cidade = CidadeRecebedora.ToUpperInvariant();
            string txid = IdentificadorTransacao;

            // -------- MONTA OS CAMPOS CONFORME ESPECIFICAÇÃO EMV ----------
            // ID 00 = Payload Format Indicator
            string payload = "000201";

            // ID 26 = Merchant Account Information
            string gui = "00" + "14" + "BR.GOV.BCB.PIX"; // GUI obrigatória
            string chave = "01" + $"{ChavePixRecebedor.Length:D2}" + ChavePixRecebedor;
            string merchantAccountInfo = gui + chave;
            payload += "26" + $"{merchantAccountInfo.Length:D2}" + merchantAccountInfo;

            // ID 52 = Merchant Category Code
            payload += "52040000";

            // ID 53 = Moeda (986 = BRL)
            payload += "5303986";

            // ID 54 = Valor (opcional, mas usado aqui)
            payload += $"54{valorFormatado.Length:D2}{valorFormatado}";

            // ID 58 = País (BR)
            payload += "5802BR";

            // ID 59 = Nome do recebedor
            payload += $"59{nome.Length:D2}{nome}";

            // ID 60 = Cidade
            payload += $"60{cidade.Length:D2}{cidade}";

            // ID 62 = Additional Data Field Template (TXID)
            string addData = "05" + $"{txid.Length:D2}" + txid;
            payload += "62" + $"{addData.Length:D2}" + addData;

            // ID 63 = CRC16 (calculado depois)
            payload += "6304";

            // Calcula o CRC16
            string crc = CalcularCrc16(payload);
            payload += crc;

            return payload;
        }

        private static string CalcularCrc16(string payload)
        {
            ushort polynomial = 0x1021;
            ushort crc = 0xFFFF;

            byte[] bytes = Encoding.ASCII.GetBytes(payload);
            foreach (byte b in bytes)
            {
                crc ^= (ushort)(b << 8);
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x8000) != 0)
                        crc = (ushort)((crc << 1) ^ polynomial);
                    else
                        crc <<= 1;
                }
            }

            return crc.ToString("X4");
        }
    }
}
