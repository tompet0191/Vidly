using System;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Vidly.mongoDB;
using Vidly.Models;
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

        [Route("Movies/New")]
        public ActionResult New()
        {
            var viewModel = new NewMovieViewModel();
            var genres = _ctx.Genres;

            viewModel.Genres = genres.Find(x => true).ToList();

            return View("New", viewModel);

        }

        public IActionResult Create(Movie movie)
        {
            //find highest movieid
            var result = _ctx.Movies.Find(x => true).SortByDescending(d => d.MovieId).Limit(1).First();

            movie.MovieId = result.MovieId + 1;
            movie.AddedDate = DateTime.Now;

            _ctx.Movies.InsertOne(movie);

            return RedirectToAction("Index", "Movies");
        }
    }
}