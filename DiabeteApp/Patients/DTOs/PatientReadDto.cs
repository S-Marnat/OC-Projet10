namespace Patients.DTOs
{
    public class PatientReadDto
    {
        public int Id { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public DateTime DateDeNaissance { get; set; }
        public string Genre { get; set; }
        public string? Adresse { get; set; }
        public string? Telephone { get; set; }
    }
}
