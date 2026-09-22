using Notes.DTOs;

namespace Notes.Services.Interfaces
{
    public interface INoteService
    {
        Task<List<NoteReadDto>> GetByPatientAsync(int idPatient);
        Task<NoteReadDto> CreateAsync(NoteCreateDto dto);
        Task<NoteReadDto?> UpdateAsync(string id, NoteUpdateDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
