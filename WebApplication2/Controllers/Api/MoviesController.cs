using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Vidly.Dtos;
using Vidly.mongoDB;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Movies")]
    public class MoviesController : Controller
    {

        private readonly IDbContext _ctx;

        public MoviesController(IDbContext ctx)
        {
            _ctx = ctx;
        }

        // GET: api/Movies
        [HttpGet]
        public IEnumerable<MovieDto> GetMovies()
        {
            return _ctx.Movies.Find(x => true).ToList().Select(Mapper.Map<Movie, MovieDto>);
        }

        // GET: api/Movies/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await _ctx.Movies.Find(x => x.MovieId == id).SingleOrDefaultAsync();
            
            if (movie == null)
                return NotFound();

            return Ok(Mapper.Map<Movie, MovieDto>(movie));
        }
        
        // POST: api/Movies
        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var maxMovieId = _ctx.Movies.Find(x => true).SortByDescending(d => d.MovieId).Limit(1).First().MovieId;

            movieDto.MovieId = ++maxMovieId;
            movieDto.AddedDate = DateTime.Now;

            var movie = Mapper.Map<MovieDto, Movie>(movieDto);

            await _ctx.Movies.InsertOneAsync(movie);

            return Created($"api/Movies/" + movieDto.MovieId, movieDto);
        }
        
        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var movieInDb = await _ctx.Movies.Find(x => x.MovieId == id).SingleOrDefaultAsync();

            if (movieInDb == null)
                return NotFound();

            var update = Builders<Movie>.Update.Set(x => x.Name, movieDto.Name)
                .Set(x => x.Genre, movieDto.Genre).Set(x => x.ReleaseDate, movieDto.ReleaseDate)
                .Set(x => x.Available, movieDto.Available);

            await _ctx.Movies.UpdateOneAsync(x => x.MovieId == id, update);

            Mapper.Map(movieInDb, movieDto);

            return Ok(movieDto);
        }
        
        // DELETE: api/Movies/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movieInDb = await _ctx.Movies.Find(x => x.MovieId == id).SingleOrDefaultAsync();

            if (movieInDb == null)
                return NotFound();

            await _ctx.Movies.DeleteOneAsync(x => x.MovieId == id);

            return NoContent();
        }
    }
}
