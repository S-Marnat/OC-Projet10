using System.ComponentModel.DataAnnotations;

namespace Front.ViewModels
{
    public class PatientEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        [MaxLength(50, ErrorMessage = "Le prénom ne doit pas dépasser 50 caractères.")]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [MaxLength(50, ErrorMessage = "Le nom ne doit pas dépasser 50 caractères.")]
        [Display(Name = "Nom")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date de naissance est obligatoire.")]
        [DataType(DataType.Date, ErrorMessage = "La date n'est pas valide.")]
        [Display(Name = "Date de naissance")]
        public DateTime DateDeNaissance { get; set; }

        [Required]
        [MaxLength(1)]
        [RegularExpression("F|M", ErrorMessage = "Le genre est obligatoire.")]
        [Display(Name = "Genre")]
        public string Genre { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "L'adresse ne doit pas dépasser 100 caractères.")]
        [Display(Name = "Adresse")]
        public string? Adresse { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Le numéro de téléphone n'est pas valide.")]
        [Display(Name = "Téléphone")]
        public string? Telephone { get; set; } = string.Empty;
    }
}
