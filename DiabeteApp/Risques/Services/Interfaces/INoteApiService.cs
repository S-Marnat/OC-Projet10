using Risques.DTOs;

namespace Risques.Services.Interfaces
{
    public interface INoteApiService
    {
        Task<List<NoteDto>> GetNotesByPatientAsync(int idPatient);
    }
}
