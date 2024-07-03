using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Fintacharts_Data_Collection.Models
{
    public class Instrument
    {
        [Key]
        public Guid id { get; set; }
        public string symbol { get; set; }
        public string kind { get; set; }
        public string description { get; set; }
        public double tickSize { get; set; }
        public string currency { get; set; }
        public string? baseCurrency { get; set; }
        public virtual ICollection<Provider> providers { get; set; }
        public virtual ICollection<InstrumentValuesTimely> instrumentValuesTimely { get; set; }
        public virtual ICollection<WebSocketMessage> webSocketMessages { get; set; }

    }
}
