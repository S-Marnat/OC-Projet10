using Risques.DTOs;
using Risques.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Risques.Services
{
    public class NoteApiService : INoteApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NoteApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<NoteDto>> GetNotesByPatientAsync(int idPatient)
        {
            var response = await SendWithRefreshAsync(() => _httpClient.GetAsync($"/note/patient/{idPatient}"));

            if (!response.IsSuccessStatusCode)
                return new List<NoteDto>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<NoteDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<NoteDto>();
        }


        // -- Méthodes utilitaires pour gérer le JWT et le RefreshToken --
        private void AddJwtHeader()
        {
            // Récupération du JWT transmis par le Gateway
            var authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authHeader))
            {
                // Nettoyage du "Bearer "
                var token = authHeader.Replace("Bearer ", "");

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private async Task<HttpResponseMessage> SendWithRefreshAsync(Func<Task<HttpResponseMessage>> action)
        {
            AddJwtHeader();
            return await action();
        }
    }
}
