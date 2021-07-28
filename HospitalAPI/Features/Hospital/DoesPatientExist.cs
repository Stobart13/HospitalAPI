using HospitalAPI.Infrastructure.Repositories.Interfaces;
using HospitalAPI.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HospitalAPI.Features.Hospital
{
    public class DoesPatientExist
    {
        public class Query : IRequest<Result>
        {
            public string Name { get; set; }
            public DateTime DateOfBirth { get; set; }
        }
        public class Result : IRequest<Unit>
        {
            public bool DoesExist { get; set; }
            public bool IsSuccessful { get; set; }
            public string ErrorMessage { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Result>
        {
            protected readonly IHospitalRepository _hospitalRepository;

            public QueryHandler(IHospitalRepository hospitalRepository)
            {
                _hospitalRepository = hospitalRepository;
            }


            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                bool isSucessful = true;
                string ErrorMessage = "";


                bool doesPatientExist = false;
                try
                {
                    doesPatientExist = await _hospitalRepository.DoesPatientExist(request.Name, request.DateOfBirth);
                }
                catch (Exception ex)
                {
                    isSucessful = false;
                    ErrorMessage = ex.Message;
                }

                return new Result
                {
                    DoesExist = doesPatientExist,
                    IsSuccessful = isSucessful,
                    ErrorMessage = ErrorMessage
                };
            }
        }
    }
}

