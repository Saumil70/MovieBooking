using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MovieBooking.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieBooking.ViewModel
{
    public class BookingDetailsViewModel
    {
        public int BookingId { get; set; }
        public int TotalSeatSelected { get; set; }
        public decimal Amount { get; set; }


        public int MovieId { get; set; }
        public int TheatreId { get; set; }
        public int Id { get; set; }

        public string UserId { get; set; }



        [ForeignKey("MovieId")]
        [ValidateNever]
        public virtual Movie Movie { get; set; }


        [ForeignKey("TheatreId")]
        [ValidateNever]
        public virtual Theatre Theatre { get; set; }

        [ForeignKey("Id")]
        [ValidateNever]
        public virtual Screen Screen { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public virtual IdentityUser IdentityUser { get; set; }
    }
}
