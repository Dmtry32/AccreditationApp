using System.ComponentModel.DataAnnotations;

namespace AccreditationApp.Models
{
    public class Accreditation
    {
        public int Id { get; set; }

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Bank { get; set; } = string.Empty;

        public string? Status { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
    }
}