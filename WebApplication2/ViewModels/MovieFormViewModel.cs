using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Vidly.Models;

namespace Vidly.ViewModels
{
    public class MovieFormViewModel
    {
        public IEnumerable<Genre> Genres { get; set; }

        public ObjectId id { get; set; }

        public int? MovieId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(255)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        [StringLength(255)]
        public string Genre { get; set; }

        [Required(ErrorMessage = "Release date is required")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ReleaseDate { get; set; }

        [Required(ErrorMessage = "The number in stock is required")]
        [Range(1, 20, ErrorMessage = "The number in stock must be between 1 and 20")]
        public int? Available { get; set; }

        public string Title => (MovieId != 0) ? "Edit Movie" : "New Movie";

        public MovieFormViewModel()
        {
            MovieId = 0;
        }

        public MovieFormViewModel(Movie movie)
        {
            MovieId = movie.MovieId;
            Name = movie.Name;
            ReleaseDate = movie.ReleaseDate;
            Available = movie.Available;
            Genre = movie.Genre;
        }
    }
}
