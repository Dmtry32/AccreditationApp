//version before fix
//using System.ComponentModel.DataAnnotations;

//namespace AccreditationApp.Models
//{
//    public class AccreditationModel
//    {
//        public int Id { get; set; }

//        [Required(ErrorMessage = "Le nom est requis")]
//        [Display(Name = "Nom")]
//        public string LastName { get; set; }

//        [Required(ErrorMessage = "Le prénom est requis")]
//        [Display(Name = "Prénom")]
//        public string FirstName { get; set; }

//        [Required(ErrorMessage = "L'adresse email est requise")]
//        [EmailAddress(ErrorMessage = "Format d'email invalide")]
//        [Display(Name = "Adresse Email")]
//        public string Email { get; set; }

//        [Required(ErrorMessage = "La date de naissance est requise")]
//        [Display(Name = "Date de naissance")]
//        [DataType(DataType.Date)]
//        public DateTime BirthDate { get; set; }

//        [Required(ErrorMessage = "La banque doit être sélectionnée")]
//        [Display(Name = "Banque")]
//        public string Bank { get; set; }

//        public string Status { get; set; }
//        public DateTime RequestDate { get; set; }
//    }
//}

//fix version
using System.ComponentModel.DataAnnotations;

namespace AccreditationApp.Models
{
    public class AccreditationModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        [Display(Name = "Nom")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Le prénom est requis")]
        [Display(Name = "Prénom")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        [Display(Name = "Adresse Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La date de naissance est requise")]
        [Display(Name = "Date de naissance")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "La banque est requise")]
        [Display(Name = "Banque")]
        public string Bank { get; set; }
    }
}