using Microsoft.AspNetCore.Mvc;
using Patients.Services.Interfaces;
using Patients.DTOs;

[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    // GET: api/Patients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientReadDto>>> GetPatients()
    {
        try
        {
            var result = await _patientService.GetAllAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Une erreur interne est survenue.");
        }
    }

    // GET: api/Patients/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PatientReadDto>> GetPatient(int id)
    {
        try
        {
            var result = await _patientService.GetByIdAsync(id);

            if (result == null)
                return NotFound("L'Id renseigné ne correspond à aucun patient.");

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Une erreur interne est survenue.");
        }
    }

    // PUT: api/Patients/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPatient(int id, PatientUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest($"Les informations fournies pour le patient Id={id} sont invalides.");

        try
        {
            var result = await _patientService.UpdateAsync(id, dto);

            if (result == null)
                return NotFound("L'Id renseigné ne correspond à aucun patient.");

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Une erreur interne est survenue.");
        }
    }

    // POST: api/Patients
    [HttpPost]
    public async Task<ActionResult<PatientReadDto>> PostPatient(PatientCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Les informations fournies pour le patient sont invalides.");

        try
        {
            var result = await _patientService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetPatient), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Une erreur interne est survenue.");
        }
    }

    // DELETE: api/Patients/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        try
        {
            var result = await _patientService.DeleteAsync(id);

            if (!result)
                return NotFound("L'Id renseigné ne correspond à aucun patient.");

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Une erreur interne est survenue.");
        }
    }
}
