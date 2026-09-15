using System.Net.Http;
using System.Net.Http.Json;
using Front.ViewModels;

namespace Front.Services
{
    public class PatientApiService
    {
        private readonly HttpClient _httpClient;

        public PatientApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PatientListViewModel>> GetAllPatientsAsync()
        {
            var patients = await _httpClient.GetFromJsonAsync<List<PatientListViewModel>>("patients");
            return patients ?? new List<PatientListViewModel>();
        }

        public async Task<PatientDetailsViewModel?> GetPatientByIdAsync(int id)
        {
            var patient = await _httpClient.GetFromJsonAsync<PatientDetailsViewModel>($"patients/{id}");
            return patient;
        }
    }
}
