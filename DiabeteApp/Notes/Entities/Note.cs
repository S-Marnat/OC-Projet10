namespace Notes.Entities
{
    public class Note
    {
        public int Id { get; set; }
        public string Contenu { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }

        public int IdPatient { get; set; }
    }
}
