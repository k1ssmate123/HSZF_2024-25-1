using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOQ9ET_HSZF_2024251.Model
{
    public class Movie
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("release_year")]
        public int ReleaseYear { get; set; }

        [JsonProperty("director")]
        public string Director { get; set; }

        [JsonProperty("box_office")]
        public long BoxOffice { get; set; }

        public Movie(string title, int releaseDate, string director, uint boxOffice)
        {
            Title = title;
            ReleaseYear = releaseDate;
            Director = director;
            BoxOffice = boxOffice;
        }
        public Movie()
        {
            
        }

        public override string ToString()
        {
            return $"Title: {Title}\n\tRelease date: {ReleaseYear}\n\tDirector: {Director}\n\tBox office: {BoxOffice}";
        }
    }
}
