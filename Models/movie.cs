
using System.ComponentModel.DataAnnotations;
using System;
using LiteDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace movie_reservation.Models
{

    public class Movie
    {
        public int Id { get; set; }
       
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Genre { get; set; }

        public string Description { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public DateTime Time1 {get; set;}
        
        [Required]
        public DateTime Time2 {get; set;}

        [Required]
        public int Seats {get; set;}

        [Required]
        public decimal Price { get; set; }
    }

  
}