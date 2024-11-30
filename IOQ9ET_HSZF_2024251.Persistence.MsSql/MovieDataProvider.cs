using IOQ9ET_HSZF_2024251.Model;
using System.Data.Entity;

namespace IOQ9ET_HSZF_2024251.Persistence.MsSql
{
    public interface IMovieDataProvider
    {
        HashSet<Movie> ListByMovie();
        void EditMovie(Movie movie, string title, int releaseYear, string director, uint boxOffice);
        ICollection<Movie> GetMoviesByDirector(string directorName);
        public event ListingEvent ListingEventHandler;

        void AddMovie(Movie movie);

    }


    public class MovieDataProvider : IMovieDataProvider
    {
        private readonly AppDbContext context;
        public event ListingEvent ListingEventHandler;

        public MovieDataProvider(AppDbContext context)
        {
            this.context = context;
        }

        public HashSet<Movie> ListByMovie()
        {
            HashSet<Movie> movieList = new HashSet<Movie>();
            ListingEventHandler.Invoke("All movies have been listed at: " + DateTime.Now);


            foreach (var movies in context.Movies.Include(x=>x.Characters))
            {
                movieList.Add(movies as Movie);
            }

            return movieList;
        }
        public void EditMovie(Movie movie, string title, int releaseYear, string director, uint boxOffice)
        {
            if (title != "")
            {
                movie.Title = title;
            }
            if (releaseYear > 0)
            {
                movie.ReleaseYear = releaseYear;
            }
            if (director != "")
            {
                movie.Director = director;
            }
            if (boxOffice > 0)
            {
                movie.BoxOffice = boxOffice;
            }
          
        }

        public void AddMovie(Movie movie)
        {
            context.Movies.Add(movie);
            context.SaveChanges();
        }
        public ICollection<Movie> GetMoviesByDirector(string directorName)
        {

            return ListByMovie().Where(x => x.Director.Contains(directorName)).ToList();

            //return context.Actors.Select(x => x.Characters.Select(y => y.Movies.Where(z => z.Director.Contains(directorName))).Select(a=>a));
        }
    }
}
