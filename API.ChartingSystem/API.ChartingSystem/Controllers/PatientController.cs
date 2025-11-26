using API.ChartingSystem.Database;
using API.ChartingSystem.Enterprise;
using Library.ChartingSystem.DTO;
using Library.ChartingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;
using System.Reflection;

namespace API.ChartingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {

        private readonly PatientEC _patientEC;

        public PatientController(PatientEC patientEC)
        {
            _patientEC = patientEC;
        }

        // GET: api/patient
        [HttpGet]
        public ActionResult<List<PatientDTO>> Get()
        {
            return Ok(_patientEC.Get());
        }

        // GET: api/patient/id
        [HttpGet("{id}")]
        public ActionResult<List<PatientDTO>> GetById(int id)
        {
            var patient = _patientEC.GetById(id);

            if (patient == null)
                return NotFound();

            return Ok(patient);
        }

        // POST: api/patient
        [HttpPost]
        public ActionResult<PatientDTO> Create([FromBody] PatientDTO dto)
        {
            var patient = _patientEC.Add(dto);

            if (patient == null) return BadRequest("Unable to create patient");

            return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
        }

        // PUT: api/patient/id
        [HttpPut("{id}")]
        public ActionResult<PatientDTO> Update(int id, [FromBody] PatientDTO dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            var patient = _patientEC.Update(dto);

            if (patient == null) return NotFound();

            return Ok(patient);
        }

        // PATCH: api/patient/id
        [HttpPatch("{id}")]
        public ActionResult<PatientDTO> Patch(int id, [FromBody] PatientUpdateDTO dto)
        {
            var patient = _patientEC.PartialUpdate(id, dto);
            if (patient == null) return NotFound();

            return Ok(patient);
        }

        // DELETE: api/patient/id
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!_patientEC.Delete(id)) return NotFound();
            return NoContent();
        }
    }
}
