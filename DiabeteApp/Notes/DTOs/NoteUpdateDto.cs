using System.ComponentModel.DataAnnotations;

namespace Notes.DTOs
{
    public class NoteUpdateDto
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        public string Contenu { get; set; } = string.Empty;
    }
}
