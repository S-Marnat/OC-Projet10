using System.ComponentModel.DataAnnotations;

namespace Notes.DTOs
{
    public class NoteCreateDto
    {
        [Required]
        public string Contenu { get; set; } = string.Empty;

        [Required]
        public int IdPatient { get; set; }
    }
}
