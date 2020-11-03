using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MvcMovie.Models
{
    public class Movie
    {
  
        public int ID { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [DisplayName ("Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [StringLength(30)]
        public string Genre { get; set; }

        [Range(1,100)]
        [DataType(DataType.Currency)]
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [StringLength(5)]
        [Required]

        public string Rating { get; set; }

        [DisplayName("Movie Director")]
        public int? DirectorID { get; set; }
        public Director Director { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Poster Name")]
        public string PosterName { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile PosterFile { get; set; }

    }
}
