using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Vidly.Models
{
    public class Rental
    {
        public ObjectId Id { get; set; }

        public int RentalId { get; set; }

        public ObjectId CustomerObjectId { get; set; }

        [Required]
        public Customer Customer { get; set; }

        public ObjectId MovieObjectId { get; set; }

        [Required]
        public Movie Movie { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DateRented { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? DateReturned { get; set; }
    }
}