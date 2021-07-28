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
    public class ListActivePatientsByWard
    {
        public class Query : IRequest<Result>
        {
            public int WardID { get; set; }
        }
        public class Result : IRequest<Unit>
        {
            public List<PatientWardReadModel> Items { get; set; }
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
                bool isSuccessful = true;
                string errorMessage = "";

                List<PatientWardReadModel> patients = new();

                try
                {
                    patients = await _hospitalRepository.ListActivePatientsByWard(request.WardID);
                }
                catch(Exception ex)
                {
                    isSuccessful = false;
                    errorMessage = ex.Message;
                }

                return new Result
                {
                    IsSuccessful = isSuccessful,
                    ErrorMessage = errorMessage,
                    Items = patients
                };
            }
        }
    }
}
