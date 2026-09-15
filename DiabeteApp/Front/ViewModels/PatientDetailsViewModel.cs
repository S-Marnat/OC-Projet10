namespace Front.ViewModels
{
    public class PatientDetailsViewModel
    {
        public int Id { get; set; }
        public string Prenom { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public DateTime DateDeNaissance { get; set; }
        public string Genre { get; set; } = string.Empty;
        public string? Adresse { get; set; }
        public string? Telephone { get; set; }
    }
}
