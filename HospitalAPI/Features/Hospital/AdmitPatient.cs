using HospitalAPI.Infrastructure.Repositories.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HospitalAPI.Features.Hospital
{
    public class AdmitPatient
    {
        public class AdmitPatientCommand : IRequest<Unit>
        {
            public Guid PatientID { get; set; }
            public int WardID { get; set; }
            public string Notes { get; set; }
            public bool IsSuccessful { get; set; }
            public string ErrorMessage { get; set; }
        }
        public class CommandHandler : IRequestHandler<AdmitPatientCommand, Unit>
        {
            private readonly IHospitalRepository _hospitalRespository;

            public CommandHandler(IHospitalRepository hospitalRepository)
            {
                _hospitalRespository = hospitalRepository;
            }

            public Task<Unit> Handle(AdmitPatientCommand command, CancellationToken cancellationToken)
            {
                command.IsSuccessful = true;

                try
                {
                    _hospitalRespository.AdmitPatient(command.PatientID, command.WardID, command.Notes);
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
