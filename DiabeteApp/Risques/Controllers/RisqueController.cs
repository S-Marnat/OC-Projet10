using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Risques.Services.Interfaces;

[Authorize(Roles = "Organisateur")]
[Route("api/[controller]")]
[ApiController]
public class RisqueController : ControllerBase
{
    private readonly IRisqueService _risqueService;
    private readonly IPatientApiService _patientApiService;

    public RisqueController(IRisqueService risqueService, IPatientApiService patientApiService)
    {
        _risqueService = risqueService;
        _patientApiService = patientApiService;
    }

    // GET: api/Risque/patient/5
    [HttpGet("patient/{id}")]
    public async Task<ActionResult> GetRisqueByPatient(int id)
    {
        var patient = await _patientApiService.GetPatientByIdAsync(id);

        if (patient == null)
            return NotFound($"Le patient avec l'ID {id} n'a pas été trouvé.");

        try
        {
            var risque = await _risqueService.EvaluerRisqueAsync(id);
            return Ok(risque);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Une erreur interne est survenue.");
        }
    }
}
