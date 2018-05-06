using System;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Vidly.mongoDB;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MovieController : Controller
    {
        private IDbContext _ctx;

        public MovieController(IDbContext ctx)
        {
            _ctx = ctx;
        }

        [Route("Movies")]
        public ActionResult Index()
        {
            var movieViewModel = new MovieViewModel();
            var movies = _ctx.Movies;
            var genres = _ctx.Genres;
            
            movieViewModel.Movies = movies.Find(x => true).ToList();
            movieViewModel.Genres = genres.Find(x => true).ToList();

            return View(movieViewModel);
        }

        [Route("Movies/Details/{id:int}")]
        public ActionResult Details(int id)
        {
            var movieViewModel = new MovieViewModel();

            var movies = _ctx.Movies;
            var genres = _ctx.Genres;

            try
            {
                movieViewModel.Movies = movies.Find(x => x.MovieId == id).ToList();
                movieViewModel.Genres = genres.Find(x => true).ToList();
            }
            catch (Exception)
            {
                return NotFound();
            }

            return View(movieViewModel);
        }
    }
}