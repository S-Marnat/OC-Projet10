namespace Front.ViewModels
{
    public class NoteListPatientViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Contenu { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }

        public int IdPatient { get; set; }
    }
}
