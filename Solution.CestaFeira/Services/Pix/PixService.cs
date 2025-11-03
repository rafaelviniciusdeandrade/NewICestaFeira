using CestaFeira.Web.Models.Pix;
using QRCoder;
using System.Drawing;
using System.Globalization;
using System.Text; // <--- 1. ADICIONADO PARA USAR ENCODING

namespace CestaFeira.Web.Services.Pix
{
    public class PixService : IPixService
    {
        // --- Constantes (Inalteradas, pois o problema não está aqui) ---
        private const string ChavePixRecebedor = "076804836205"; // 12 caracteres (mas forçamos 0111)
        private const string NomeRecebedor = "Marlon";
        private const string CidadeRecebedora = "Alfenas";
        private const string IdentificadorTransacao = "CestaFeira";

        // --- Método Público para o Controller (Sem Alteração) ---
        public async Task<PixResponse> GerarQrCodePix(decimal valor, string cpfOuCnpjProdutor)
        {
            string pixPayload = ObterPayloadPixSimulado(valor);

            if (string.IsNullOrEmpty(pixPayload))
            {
                return new PixResponse { Success = false, Message = "Payload Pix (Copia e Cola) não foi gerado corretamente." };
            }

            try
            {
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(pixPayload, QRCodeGenerator.ECCLevel.Q);

                    using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                    {
                        byte[] qrCodeBytes = qrCode.GetGraphic(pixelsPerModule: 20);
                        string qrCodeImageAsBase64 = Convert.ToBase64String(qrCodeBytes);

                        return new PixResponse
                        {
                            Success = true,
                            QrCodeBase64 = qrCodeImageAsBase64,
                            PixCode = pixPayload,
                            Message = "QR Code gerado com sucesso."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new PixResponse { Success = false, Message = $"Erro interno na geração do QR Code: {ex.Message}" };
            }
        }


        // --- Método Privado: Montagem e Geração do Payload PIX ---
        private string ObterPayloadPixSimulado(decimal valor)
        {
            // 2. AJUSTE: HARDCODED o valor (12.00) para eliminar erros de formatação local.
            // Se o seu valor for diferente de 12.00, mude aqui e o valor da string.length.
            string valorFormatado = "12.00";

            // 3. Cria o ID de Transação
            string infoAdicional = $"0510{IdentificadorTransacao}";

            // 4. Monta o payload BÁSICO
            string payloadBase =
                // 00 - Versão
                $"000201" +
                // 26 - Bloco PIX (?? será substituído por 33)
                $"26??0014BR.GOV.BCB.PIX" +
                    // 01 - Chave PIX: Força o tamanho 11
                    $"0111{ChavePixRecebedor}" +
                // 52 e 53 - Merchant Category Code e Moeda
                $"52040000" +
                $"5303986" +
                // 54 - Valor (Length: 5 para "12.00")
                $"54{valorFormatado.Length:D2}{valorFormatado}" +
                // 58 - País (BR)
                $"5802BR" +
                // 59 - Nome
                $"59{NomeRecebedor.Length:D2}{NomeRecebedor}" +
                // 60 - Cidade
                $"60{CidadeRecebedora.Length:D2}{CidadeRecebedora}" +
                // 62 - Informação Adicional/TXID
                $"62{infoAdicional.Length:D2}{infoAdicional}";

            // 5. Inserção do tamanho do bloco 26 (Força 33)
            payloadBase = payloadBase.Replace("26??", "2633");

            // 6. Calcula o CRC16 para o Payload final
            string payloadComTagCrc = $"{payloadBase}6304";
            string crc16 = CalcularCrc16(payloadComTagCrc);

            // 7. Retorna o Payload completo
            return $"{payloadComTagCrc}{crc16}";
        }

        // --- Método Privado: Cálculo do CRC16 (CORRIGIDO PARA UTF-8) ---
        private static string CalcularCrc16(string payload)
        {
            // Tabela completa de 256 valores para CRC-16-CCITT
            ushort[] Crc16Table = {
                0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50A5, 0x60C6, 0x70E7, 0x8108, 0x9129, 0xA14A, 0xB16B, 0xC18C, 0xD1AD, 0xE1CE, 0xF1EF,
                0x1231, 0x0210, 0x3273, 0x2252, 0x52B5, 0x4294, 0x72F7, 0x62D6, 0x9339, 0x8318, 0xB37B, 0xA35A, 0xD3BD, 0xC39C, 0xF3FF, 0xE3DE,
                0x2462, 0x3443, 0x0420, 0x1401, 0x64E6, 0x74C7, 0x44A4, 0x5485, 0xA56A, 0xB54B, 0x8528, 0x9509, 0xE5EE, 0xF5CF, 0xC5AC, 0xD58D,
                0x3653, 0x2672, 0x1611, 0x0630, 0x76D7, 0x66F6, 0x5695, 0x46B4, 0xB75B, 0xA77A, 0x9719, 0x8738, 0xF7DF, 0xE7FE, 0xD79D, 0xC7BC,
                0x48C4, 0x58E5, 0x6886, 0x78A7, 0x0840, 0x1861, 0x2802, 0x3823, 0xC9CC, 0xD9ED, 0xE98E, 0xF9AF, 0x8948, 0x9969, 0xA90A, 0xB92B,
                0x5AF5, 0x4AD4, 0x7AB7, 0x6A96, 0x1A71, 0x0A50, 0x3A33, 0x2A12, 0xDBFD, 0xCBDC, 0xFBBF, 0xEB9E, 0x9B79, 0x8B58, 0xBB3B, 0xAB1A,
                0x6CA6, 0x7C87, 0x4CE4, 0x5CC5, 0x2C22, 0x3C03, 0x0C60, 0x1C41, 0xEDAE, 0xFD8F, 0xCDEC, 0xDDCD, 0xAD2A, 0xBD0B, 0x8D68, 0x9D49,
                0x7E97, 0x6EB6, 0x5ED5, 0x4EF4, 0x3E13, 0x2E32, 0x1E51, 0x0E70, 0xFF9F, 0xEFBE, 0xDFDD, 0xCFFC, 0xBF1B, 0xAF3A, 0x9F59, 0x8F78,
                0x9188, 0x81A9, 0xB1CA, 0xA1EB, 0xD10C, 0xC12D, 0xF14E, 0xE16F, 0x1080, 0x00A1, 0x30C2, 0x20E3, 0x5004, 0x4025, 0x7046, 0x6067,
                0x83B9, 0x9398, 0xA3FB, 0xB3DA, 0xC33D, 0xD31C, 0xE37F, 0xF35E, 0x02B1, 0x1290, 0x22F3, 0x32D2, 0x4235, 0x5214, 0x6277, 0x7256,
                0xB5EA, 0xA5CB, 0x95A8, 0x8589, 0xF56E, 0xE54F, 0xD52C, 0xC50D, 0x34E2, 0x24C3, 0x14A0, 0x0481, 0x7466, 0x6447, 0x5424, 0x4405,
                0xA7DB, 0xB7FA, 0x8799, 0x97B8, 0xE75F, 0xF77E, 0xC71D, 0xD73C, 0x26D3, 0x36F2, 0x0691, 0x16B0, 0x6657, 0x7676, 0x4615, 0x5634,
                0xD94C, 0xC96D, 0xF90E, 0xE92F, 0x99C8, 0x89E9, 0xB98A, 0xA9AB, 0x5844, 0x4865, 0x7806, 0x6827, 0x18C0, 0x08E1, 0x3882, 0x28A3,
                0xEB53, 0xFB72, 0xCBB1, 0xDB90, 0xAB77, 0xBB56, 0x8B35, 0x9B14, 0x4AFB, 0x5ADF, 0x6AB9, 0x7A98, 0x0A71, 0x1A50, 0x2A33, 0x3A12,
                0xFD3E, 0xED1F, 0xDD7C, 0xCD5D, 0xBDAA, 0xAD8B, 0x9DE8, 0x8DC9, 0x7C26, 0x6C07, 0x5C64, 0x4C45, 0x3CA2, 0x2C83, 0x1CE0, 0x0CC1,
                0xEF1F, 0xFF3E, 0xCF5D, 0xDF7C, 0xAF9B, 0xBFBA, 0x8FD9, 0x9FF8, 0x6E17, 0x7E36, 0x4E55, 0x5E74, 0x2E93, 0x3EB2, 0x0ED1, 0x1EF0
            };

            ushort crc = 0xFFFF;

            // 🚨 CORREÇÃO CRÍTICA: Conversão explícita para bytes usando UTF8
            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

            foreach (byte b in payloadBytes)
            {
                byte index = (byte)(((crc >> 8) ^ b) & 0xFF);
                crc = (ushort)((crc << 8) ^ Crc16Table[index]);
            }

            return crc.ToString("X4");
        }
    }
}