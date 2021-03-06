﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Vidly.Dtos
{
    public class MovieDto
    {
        public int MovieId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(255)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        [StringLength(255)]
        public string Genre { get; set; }

        [Required(ErrorMessage = "Release date is required")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ReleaseDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime AddedDate { get; set; }

        [Required(ErrorMessage = "The number in stock is required")]
        [Range(1, 20, ErrorMessage = "The number in stock must be between 1 and 20")]
        public int NumberInStock { get; set; }
    }
}
