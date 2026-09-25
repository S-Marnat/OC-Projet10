namespace Risques.DTOs
{
    public class NoteDto
    {
        public string Id { get; set; } = string.Empty;
        public string Contenu { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }

        public int IdPatient { get; set; }
    }
}
