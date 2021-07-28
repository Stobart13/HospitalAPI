using HospitalAPI.Infrastructure.Repositories.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HospitalAPI.Features.Hospital
{
    public class DischargePatient
    {
        public class DischargePatientCommand : IRequest<Unit>
        {
            public Guid SpellID { get; set; }
            public Guid PatientID { get; set; }
            public bool IsSuccessful { get; set; }
            public string ErrorMessage { get; set; }
        }
        public class CommandHandler : IRequestHandler<DischargePatientCommand, Unit>
        {
            private readonly IHospitalRepository _hospitalRespository;

            public CommandHandler(IHospitalRepository hospitalRepository)
            {
                _hospitalRespository = hospitalRepository;
            }

            public Task<Unit> Handle(DischargePatientCommand command, CancellationToken cancellationToken)
            {
                command.IsSuccessful = true;

                try
                {
                    _hospitalRespository.DischargePatient(command.PatientID, command.SpellID) ;
                }
                catch (Exception ex)
                {
                    command.IsSuccessful = false;
                    command.ErrorMessage = ex.Message;
                }

                return Unit.Task;
            }
        }
    }
}
