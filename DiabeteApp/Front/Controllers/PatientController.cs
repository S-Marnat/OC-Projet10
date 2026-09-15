using Front.Services;
using Microsoft.AspNetCore.Mvc;
using Front.ViewModels;

namespace Front.Controllers
{
    public class PatientController : Controller
    {
        private readonly PatientApiService _patientApiService;

        public PatientController(PatientApiService patientApiService)
        {
            _patientApiService = patientApiService;
        }

        // GET: Patient
        public async Task<IActionResult> Index()
        {
            var patients = await _patientApiService.GetAllPatientsAsync();
            return View(patients);
        }

        // GET: Patient/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var patient = await _patientApiService.GetPatientByIdAsync(id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        // GET: Patient/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _patientApiService.CreatePatientAsync(model);
            return RedirectToAction("Index");
        }

        // GET: Patient/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _patientApiService.GetPatientByIdAsync(id);

            if (patient == null)
                return NotFound();

            var model = new PatientEditViewModel
            {
                Id = patient.Id,
                Prenom = patient.Prenom,
                Nom = patient.Nom,
                DateDeNaissance = patient.DateDeNaissance,
                Genre = patient.Genre,
                Adresse = patient.Adresse,
                Telephone = patient.Telephone
            };

            return View(model);
        }

        // POST: Patient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _patientApiService.UpdatePatientAsync(id, model);
            return RedirectToAction("Index");
        }

        // GET: Patient/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _patientApiService.GetPatientByIdAsync(id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _patientApiService.DeletePatientAsync(id);
            return RedirectToAction("Index");
        }
    }
}
