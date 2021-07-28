using HospitalAPI.Infrastructure.Repositories.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HospitalAPI.Features.Hospital
{
    public class UpdatePatient
    {
        public class UpdatePatientCommand : IRequest<Unit>
        {
            public Guid PatientID { get; set; }
            public string Name { get; set; }
            public string Gender { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public bool IsSuccessful { get; set; }
            public string ErrorMessage { get; set; }
        }
        public class CommandHandler : IRequestHandler<UpdatePatientCommand, Unit>
        {
            private readonly IHospitalRepository _hospitalRespository;

            public CommandHandler(IHospitalRepository hospitalRepository)
            {
                _hospitalRespository = hospitalRepository;
            }

            public Task<Unit> Handle(UpdatePatientCommand command, CancellationToken cancellationToken)
            {
                command.IsSuccessful = true;

                try
                {
                    _hospitalRespository.UpdatePatient(command.PatientID, command.Name, command.Gender, command.DateOfBirth);
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
