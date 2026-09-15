namespace Front.ViewModels
{
    public class PatientListViewModel
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public DateTime DateDeNaissance { get; set; }
    }
}
