using Risques.DTOs;

namespace Risques.Services.Interfaces
{
    public interface IPatientApiService
    {
        Task<PatientDto?> GetPatientByIdAsync(int id);
    }
}
