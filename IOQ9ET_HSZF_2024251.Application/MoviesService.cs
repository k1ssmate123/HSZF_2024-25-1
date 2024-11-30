using IOQ9ET_HSZF_2024251.Model;
using IOQ9ET_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOQ9ET_HSZF_2024251.Application
{
    public interface IMovies
    {
        HashSet<Movie> ListByMovie();
        void EditMovie(Movie movie, string title, int releaseYear, string director, uint boxOffice);
        ICollection<Movie> GetMoviesByDirector(string directorName);
        void AddMovie(Movie movie);
    }

    public class MoviesService : IMovies
    {
        private readonly IMovieDataProvider MovieDataProvider;
        public MoviesService(IMovieDataProvider MovieDataProvider)
        {
            this.MovieDataProvider = MovieDataProvider;
            string path = Path.Combine(Directory.GetCurrentDirectory(), DateTime.Now.Month.ToString());
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, DateTime.Now.Day.ToString() + ".txt");
            MovieDataProvider.ListingEventHandler += x => File.WriteAllText(path, x);
        }
        public void EditMovie(Movie movie, string title, int releaseYear, string director, uint boxOffice)
        {
            MovieDataProvider.EditMovie(movie,title,releaseYear,director,boxOffice);
        }
        public void AddMovie(Movie movie)
        {
            MovieDataProvider.AddMovie(movie);
        }
        public ICollection<Movie> GetMoviesByDirector(string directorName)
        {
            return MovieDataProvider.GetMoviesByDirector(directorName);
        }

        public HashSet<Movie> ListByMovie()
        {
            return MovieDataProvider.ListByMovie();
        }

        
    }
}
