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
            var genres = _ctx.Genres;

            var viewModel = new MovieFormViewModel
            {
                Movie = new Movie(),
                Genres = genres.Find(x => true).ToList()
            };

            return View("MovieForm", viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Movies/Save")]
        public IActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new MovieFormViewModel()
                {
                    Movie = movie,
                    Genres = _ctx.Genres.Find(x => true).ToList()
                };

                return View("MovieForm", viewModel);
            }

            if(movie.MovieId == 0)
            {
                //find highest movieid
                var result = _ctx.Movies.Find(x => true).SortByDescending(d => d.MovieId).Limit(1).First();

                movie.MovieId = result.MovieId + 1;
                movie.AddedDate = DateTime.Now;

                _ctx.Movies.InsertOneAsync(movie);
            }
            else
            {
                var update = Builders<Movie>.Update.Set(x => x.Name, movie.Name)
                    .Set(x => x.ReleaseDate, movie.ReleaseDate)
                    .Set(x => x.Genre, movie.Genre)
                    .Set(x => x.Available, movie.Available);

                _ctx.Movies.UpdateOneAsync(x => x.MovieId == movie.MovieId, update);
            }

            return RedirectToAction("Index", "Movies");
        }

        [Route("Movies/Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var movie = _ctx.Movies.Find(x => x.MovieId == id).First();

            if (movie == null)
                return NotFound();

            var viewModel = new MovieFormViewModel()
            {
                Movie = movie,
                Genres = _ctx.Genres.Find(x => true).ToList()
            };

            return View("MovieForm", viewModel);
        }
    }
}