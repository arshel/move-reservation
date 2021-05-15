using System;
using LiteDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace movie_reservation.Pages
{
 public class ReservationIndexModel : PageModel
{

        private readonly ILogger<ReservationIndexModel> _logger;

        public ReservationIndexModel(ILogger<ReservationIndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet() {}
  }
}
