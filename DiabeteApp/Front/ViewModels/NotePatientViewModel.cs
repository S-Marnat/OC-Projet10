namespace Front.ViewModels
{
    public class NotePatientViewModel
    {
        public int IdPatient { get; set; }
        public List<NoteListPatientViewModel> Notes { get; set; }
    }
}
