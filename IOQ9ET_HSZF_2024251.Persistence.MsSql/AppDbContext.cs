using IOQ9ET_HSZF_2024251.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IOQ9ET_HSZF_2024251.Persistence.MsSql
{

    public class JsonObject
    {
        [JsonProperty("actors")]
        public List<Actor> Actors { get; set; }
    }

    public class AppDbContext
    {
        [JsonProperty("actors")]
        public List<Actor> Actors { get; set; }

        public AppDbContext()
        {
            Actors = new List<Actor>();
            

        }

        public void ReadJson()
        {

            var rootObject = JsonConvert.DeserializeObject<JsonObject>(File.ReadAllText("movies.json"));
            Actors = rootObject.Actors;
           

        }
    }
}
