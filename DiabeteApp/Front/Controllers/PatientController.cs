using Front.Services;
using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers
{
    public class PatientController : Controller
    {
        private readonly PatientApiService _patientApiService;

        public PatientController(PatientApiService patientApiService)
        {
            _patientApiService = patientApiService;
        }

        public async Task<IActionResult> Index()
        {
            var patients = await _patientApiService.GetAllPatientsAsync();
            return View(patients);
        }

        public async Task<IActionResult> Details(int id)
        {
            var patient = await _patientApiService.GetPatientByIdAsync(id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }
    }
}
