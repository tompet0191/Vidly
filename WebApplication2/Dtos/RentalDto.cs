using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Vidly.Dtos
{
    public class RentalDto
    {
        public int RentalId { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        public CustomerDto Customer { get; set; }

        [Required(ErrorMessage = "Movie is required")]
        public MovieDto Movie { get; set; }

        [Required(ErrorMessage = "Rented date is required")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DateRented { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? DateReturned { get; set; }
    }
}
