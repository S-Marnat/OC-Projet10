using Risques.DTOs;

namespace Risques.Services.Interfaces
{
    public interface IRisqueService
    {
        Task<RisqueDto> EvaluerRisqueAsync(int idPatient);
        Task<int> CalculerAgePatientAsync(DateTime dateDeNaissance);
        Task<int> CalculerNombreDeclencheursAsync(List<NoteDto> notes);
    }
}
