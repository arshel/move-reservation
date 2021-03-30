using LiteDB;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace movie_reservation.Models
{

    public class Movie
    {
        public int Id { get; set; }
       
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Genre { get; set; }

        // [Required]
        // public string Description { get; set; }

        [Required]
        public string Image { get; set; }
        
        [Required]
        public decimal Price { get; set; }
    }

  
}