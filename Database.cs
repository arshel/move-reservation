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

namespace movie_reservation
{

    public class Movie
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
}

    public class Database
    {
        public static void database ()
        {
         
            using (var db = new LiteDatabase(@"movieReservation.db"))
            {
                  
            }
           
        }
   
    }
}