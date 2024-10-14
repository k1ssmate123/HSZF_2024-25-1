using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOQ9ET_HSZF_2024251.Model
{
    public class Movie
    {
        public string Title { get; private set; }
        public int ReleaseDate { get; private set; }
        public string Director { get; private set; }
        public uint BoxOffice { get; private set; }

        public Movie(string title, int releaseDate, string director, uint boxOffice)
        {
            Title = title;
            ReleaseDate = releaseDate;
            Director = director;
            BoxOffice = boxOffice;
        }
        public Movie()
        {
            
        }

        public override string ToString()
        {
            return $"Title: {Title}\n\tRelease date: {ReleaseDate}\n\tDirector: {Director}\n\tBox office: {BoxOffice}";
        }
    }
}
