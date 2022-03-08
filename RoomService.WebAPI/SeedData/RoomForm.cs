using System;
using Newtonsoft.Json;

namespace RoomService.WebAPI.SeedData
{
    public class RoomForm
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("floor")]
        public int Floor { get; set; }

        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; set; }

        [JsonProperty("publishedDate")]
        public DateTime AddedDate { get; set; }
    }
}
