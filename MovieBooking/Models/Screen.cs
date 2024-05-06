using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieBooking.Models
{
    public class Screen
    {
        [Key]
        public int Id { get; set; } 
        public int ScreenId { get; set; }
        public int MovieId { get; set; }
        public int TheatreId { get; set; }
        public int TotalSeats { get; set; }
        public int RemainingSeats { get; set; }

        [Required(ErrorMessage = "Showtime is required")]
        [Display(Name = "Showtime")]
        [DataType(DataType.DateTime)]
        public DateTime Showtime { get; set; }

        [ForeignKey("MovieId")]

        [ValidateNever]
        public virtual Movie Movie { get; set; }

        [ForeignKey("TheatreId")]

        [ValidateNever]
        public virtual Theatre Theatre { get; set; }
    }
}
