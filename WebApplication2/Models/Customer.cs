using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Vidly.Models
{
    [BsonIgnoreExtraElements]
    public class Customer
    {
        public ObjectId Id { get; set; }

        public int CustomerId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(255)]
        public string LastName { get; set; }
        
        public bool IsSubscribedToNewsLetter { get; set; }

        [Required(ErrorMessage = "Select a membership type")]
        public string MembershipType { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? BirthDate { get; set; }
    }
}
