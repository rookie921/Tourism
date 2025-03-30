using System.ComponentModel.DataAnnotations;

namespace tourism02.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public required string Destination { get; set; }

        [Required]
        public int NumberOfGuests { get; set; }

        [Required]
        public required string FullName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Phone { get; set; }
    }
}
