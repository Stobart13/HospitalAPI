using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalAPI.Models;

namespace HospitalAPI.Infrastructure.Repositories.Interfaces
{
    public interface IHospitalRepository
    {
        Task<List<PatientReadModel>> ListActivePatients();
        Task<List<PatientWardReadModel>> ListActivePatientsByWard(int WardID);
        void AddPatient(string Name, string Gender, DateTime DateOfBirth);
        void UpdatePatient(Guid PatientID, string Name, string Gender, DateTime? DateOfBirth);
        void AdmitPatient(Guid PatientID, int WardID, string Notes);
        void DischargePatient(Guid PatientID, Guid SpellID);
        Task<bool> IsPatientAdmitted(Guid PatientID);
        Task<bool> DoesPatientExist(string Name, DateTime DateOfBirth);
    }
}
