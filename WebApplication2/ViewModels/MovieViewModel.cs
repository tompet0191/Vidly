using System.Collections.Generic;
using Vidly.Models;

namespace Vidly.ViewModels
{
    public class MovieViewModel
    {
        public List<Movie> Movies { get; set; }
        public List<Genre> Genres { get; set; }

        public MovieViewModel()
        {
           Movies = new List<Movie>();
           Genres = new List<Genre>();
        }
    }
}
