
using Newtonsoft.Json;

namespace Fintacharts_Data_Collection.Models
{
    public class WebSocketMessage
    {
        public string type { get; set; }
        public string id { get; set; }
        public string instrumentalId { get; set; }
        public string provider { get; set; }
        public bool subscribe { get; set; }
        public List<string> kinds { get; set; }
        [JsonIgnore]
        public Instrument instrument { get; set; }
    }
}
