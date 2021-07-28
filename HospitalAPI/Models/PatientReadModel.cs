using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAPI.Models
{
    public class PatientReadModel
    {
        public Guid PatientID { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public bool IsAdmitted { get; set; }
    }
}
