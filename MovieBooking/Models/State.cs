
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MovieBooking.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }    
        public string StateName { get; set; }

        public int CountryId { get; set; }

        // Navigation property for Country
        [ForeignKey("CountryId")]
        [ValidateNever]
        public virtual Countries Country { get; set; }

        
    }
}
