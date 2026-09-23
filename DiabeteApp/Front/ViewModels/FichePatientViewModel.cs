namespace Front.ViewModels
{
    public class FichePatientViewModel
    {
        public PatientDetailsViewModel Patient { get; set; }
        public List<NoteListPatientViewModel> Notes { get; set; }
    }
}
