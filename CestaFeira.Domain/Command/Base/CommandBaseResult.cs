using System.Text.Json.Serialization;


namespace CestaFeira.Domain.Command.Base
{
    public class CommandBaseResult
    {
        internal T Convert<T>()
        {
            throw new NotImplementedException();
        }
        [JsonPropertyName("result")]
        public Object Result { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
