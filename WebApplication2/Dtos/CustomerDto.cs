using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Vidly.Dtos
{
    public class CustomerDto
    {
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

        public MembershipTypeDto MembershipTypeDto { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        //[Min18YearsIfAMember]
        public DateTime? BirthDate { get; set; }
    }
}