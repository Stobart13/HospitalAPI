using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalAPI.Features.Hospital;
using HospitalAPI.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHospitalRepository _hospitalRepository;

        public HospitalController(IMediator mediator, IHospitalRepository hospitalRepository)
        {
            _mediator = mediator;
            _hospitalRepository = hospitalRepository;
        }

        [HttpGet("ListActivePatients")]
        public async Task<IActionResult> ListActivePatients([FromQuery] ListActivePatients.Query query)
        {
            var model = await _mediator.Send(query);

            if (!model.IsSuccessful)
                return StatusCode(500, model.ErrorMessage);

            return Ok(model.Items);
        }
        [HttpGet("ListActivePatientsByWard")]
        public async Task<IActionResult> ListActivePaByWardtients([FromQuery] ListActivePatientsByWard.Query query)
        {
            var model = await _mediator.Send(query);

            if (!model.IsSuccessful)
                return StatusCode(500, model.ErrorMessage);

            return Ok(model.Items);
        }
        [HttpGet("DoesPatientExist")]
        public async Task<IActionResult> DoesPatientExist([FromQuery] DoesPatientExist.Query query)
        {
            var model = await _mediator.Send(query);

            if (!model.IsSuccessful)
                return StatusCode(500, model.ErrorMessage);

            return Ok(model.DoesExist);
        }
        [HttpPost("AddPatient")]
        public async Task<IActionResult> AddPatient([FromBody] AddPatient.PatientCommand command)
        {
            await _mediator.Send(command);

            if (!command.IsSuccessful)
                return StatusCode(500, command.ErrorMessage);

            return Ok(command.IsSuccessful);
        }
        [HttpPost("AdmitPatient")]
        public async Task<IActionResult> AdmitPatient([FromBody] AdmitPatient.AdmitPatientCommand command)
        {
            await _mediator.Send(command);

            if (!command.IsSuccessful)
                return StatusCode(500, command.ErrorMessage);

            return Ok(command.IsSuccessful);
        }
        [HttpPost("DischargePatient")]
        public async Task<IActionResult> DischargePatient([FromBody] DischargePatient.DischargePatientCommand command)
        {
            await _mediator.Send(command);

            if (!command.IsSuccessful)
                return StatusCode(500, command.ErrorMessage);

            return Ok(command.IsSuccessful);
        }
        [HttpPost("UpdatePatient")]
        public async Task<IActionResult> UpdatePatient([FromBody] UpdatePatient.UpdatePatientCommand command)
        {
            await _mediator.Send(command);

            if (!command.IsSuccessful)
                return StatusCode(500, command.ErrorMessage);

            return Ok(command.IsSuccessful);
        }
        [HttpGet("IsPatientAdmitted")]
        public async Task<IActionResult> IsPatientAdmitted([FromQuery] IsPatientAdmitted.Query query)
        {
            var model = await _mediator.Send(query);

            if (!model.IsSuccessful)
                return StatusCode(500, model.ErrorMessage);

            return Ok(model.IsAdmitted);
        }
    }
}