namespace CestaFeira.Web.Helpers
{
    public static class ImagemHelper
    {
        public static byte[] ToByteArray(this IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return new byte[0];
            }

            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
