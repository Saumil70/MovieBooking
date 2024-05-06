
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MovieBooking.Models
{
    public class City
    {
        public int CityId { get; set; } 
        public string CityName { get; set; }

        public int StateId { get; set; }    
        // Navigation property for Country
        [ForeignKey("StateId")]

        [ValidateNever]
        public virtual State States { get; set; }
    }
}
        