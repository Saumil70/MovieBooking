using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MovieBooking.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieBooking.ViewModel
{
    public class ScreenViewModel
    {
        public int Id { get; set; }
        public int ScreenId { get; set; }
        public int MovieId { get; set; }
        public int TheatreId { get; set; }
        public int TotalSeats { get; set; }

        public int RemainingSeats {  get; set; }

        // Define ShowTime as a collection of DateTime objects
        public List<DateTime> ShowTime { get; set; }

        [ForeignKey("MovieId")]
        [ValidateNever]
        public virtual Movie Movie { get; set; }

        [ForeignKey("TheatreId")]
        [ValidateNever]
        public virtual Theatre Theatre { get; set; }
    }

}
