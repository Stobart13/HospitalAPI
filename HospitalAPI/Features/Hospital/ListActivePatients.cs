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
    public class ListActivePatients
    {
        public class Query : IRequest<Result>
        {

        }
        public class Result : IRequest<Unit>
        {
            public List<PatientReadModel> Items { get; set; }
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


                List<PatientReadModel> patients = new();
                try
                {
                    patients = await _hospitalRepository.ListActivePatients();
                }
                catch (Exception ex)
                {
                    isSucessful = false;
                    ErrorMessage = ex.Message;
                }

                return new Result
                {
                    Items = patients,
                    IsSuccessful = isSucessful,
                    ErrorMessage = ErrorMessage
                };
            }
        }
    }
}

