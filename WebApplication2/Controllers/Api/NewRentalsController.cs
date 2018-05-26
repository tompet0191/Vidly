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
    [Route("api/Rentals")]
    public class NewRentalsController : Controller
    {
        private readonly IDbContext _ctx;

        public NewRentalsController(IDbContext ctx)
        {
            _ctx = ctx;
        }

        // POST: api/Rentals
        [HttpPost]
        public IActionResult NewRental([FromBody] NewRentalDto newRentalDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model state is not valid");

            var rentalWithMaxRentalId = _ctx.Rentals.Find(x => true).SortByDescending(d => d.RentalId).Limit(1).FirstOrDefault();
            var maxRentalId = (rentalWithMaxRentalId == null) ? 0 : rentalWithMaxRentalId.RentalId;

            var customer = _ctx.Customers.Find(x => x.CustomerId == newRentalDto.CustomerId).Single();

            var movies = _ctx.Movies.Find(x => newRentalDto.MovieIds.Contains(x.MovieId)).ToList(); //select * from table where movieid in (1,2,3)

            foreach (var movie in movies)
            {
                if(movie.Available == 0)
                    return BadRequest("Movie is not available.");

                movie.Available--;

                var rentalDto = new RentalDto
                {
                    RentalId = ++maxRentalId,
                    DateRented = DateTime.Now,
                    Customer = Mapper.Map<Customer, CustomerDto>(customer),

                    Movie = Mapper.Map<Movie, MovieDto>(movie)
                };
                var rental = Mapper.Map<RentalDto, Rental>(rentalDto);

                rental.CustomerObjectId = rental.Customer.Id = customer.Id;
                rental.MovieObjectId = rental.Movie.Id = movie.Id;

                var update = Builders<Movie>.Update.Set(x => x.Available, movie.Available);

                _ctx.Movies.UpdateOne(x => x.MovieId == movie.MovieId, update);
                _ctx.Rentals.InsertOne(rental);
                
            }
 
            return Ok();
        }

        // GET: api/Rentals
        [HttpGet]
        public IEnumerable<RentalDto> GetRentals()
        {
            var rentals = _ctx.Rentals.Find(x => true).ToList();

            foreach (var rental in rentals)
            {
                rental.Customer = _ctx.Customers.Find(x => x.Id == rental.CustomerObjectId).SingleOrDefault();
                rental.Movie = _ctx.Movies.Find(x => x.Id == rental.MovieObjectId).SingleOrDefault();
            }

            var rentalDtos = Mapper.Map<IEnumerable<Rental>, IEnumerable<RentalDto>>(rentals);
            foreach (var rentalDto in rentalDtos)
            {
                rentalDto.Customer.MembershipTypeDto = new MembershipTypeDto
                {
                    MembershipType = rentalDto.Customer.MembershipType,
                    Name = _ctx.MembershipTypes.Find(x => x.Id == rentalDto.Customer.MembershipType).SingleOrDefault().Name
                };
            }

            return rentalDtos;
        }
    }
}