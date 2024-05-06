using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MovieBooking.Models
{
    public class Movie
    {
        public int MovieId { get; set; } 
        public string MovieName { get; set; }   
        public string Genre { get; set; }   
        public TimeSpan Time {  get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }


    }
}
