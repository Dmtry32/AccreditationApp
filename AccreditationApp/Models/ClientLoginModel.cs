using System.ComponentModel.DataAnnotations;

namespace AccreditationApp.Models
{
    public class ClientLoginModel
    {
        [Required(ErrorMessage = "L'adresse email est requise")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        [Display(Name = "Adresse email")]
        public string Email { get; set; }

        [Display(Name = "Code de vérification")]
        public string VerificationCode { get; set; }

        public bool RequiresVerification { get; set; }
    }
}