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

        public async Task<bool> CreatePatientAsync(PatientCreateViewModel patient)
        {
            var response = await _httpClient.PostAsJsonAsync("patients", patient);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdatePatientAsync(int id, PatientEditViewModel patient)
        {
            var response = await _httpClient.PutAsJsonAsync($"patients/{id}", patient);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"patients/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
