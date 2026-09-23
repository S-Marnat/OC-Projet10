using System.ComponentModel.DataAnnotations;

namespace Front.ViewModels
{
    public class NoteCreateViewModel
    {
        [Required(ErrorMessage = "Le contenu de la note est requis.")]
        public string Contenu { get; set; } = string.Empty;

        [Required]
        public int IdPatient { get; set; }
    }
}
