using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IOQ9ET_HSZF_2024251.Model
{
    public class Movie
    {
        [XmlIgnore]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("release_year")]
        public int ReleaseYear { get; set; }

        [JsonProperty("director")]
        public string Director { get; set; }

        [JsonProperty("box_office")]
        public long BoxOffice { get; set; }

        [XmlIgnore]
        public virtual ICollection<Character> Characters{ get; set; }

        public Movie(string title, int releaseDate, string director, uint boxOffice)
        {
            Title = title;
            ReleaseYear = releaseDate;
            Director = director;
            BoxOffice = boxOffice;
          
            Characters = new HashSet<Character>();
        }
        public Movie()
        {
            Characters = new HashSet<Character>();
          
        }

        public override string ToString()
        {
            return $"Title: {Title}\n\tRelease date: {ReleaseYear}\n\tDirector: {Director}\n\tBox office: {BoxOffice}";
        }

       
    }
}
