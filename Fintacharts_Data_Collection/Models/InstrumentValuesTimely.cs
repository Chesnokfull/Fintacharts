using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fintacharts_Data_Collection.Models
{
    public class InstrumentValuesTimely
    {
        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime t { get; set; }
        public float o { get; set; }
        public float h { get; set; }
        public float l { get; set; }
        public float c { get; set; }
        public int v { get; set; }
        [JsonIgnore]
        public Instrument instrument { get; set; }
    }
}
