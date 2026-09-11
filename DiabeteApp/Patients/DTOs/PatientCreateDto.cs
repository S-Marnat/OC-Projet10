using System.ComponentModel.DataAnnotations;

namespace Patients.DTOs
{
    public class PatientCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Prenom { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nom { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateDeNaissance { get; set; }

        [Required]
        [MaxLength(1)]
        public string Genre { get; set; }

        [MaxLength(100)]
        public string? Adresse { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string? Telephone { get; set; }
    }
}
