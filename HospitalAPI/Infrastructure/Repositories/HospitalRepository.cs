using HospitalAPI.Infrastructure.Repositories.Interfaces;
using HospitalAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAPI.Infrastructure.Repositories
{
    public class HospitalRepository : IHospitalRepository
    {

        protected readonly string _hospitalConnectionString;

        public HospitalRepository(string HospitalConnectionString)
        {
            _hospitalConnectionString = HospitalConnectionString;
        }

        public async void AddPatient(string Name, string Gender, DateTime DateOfBirth)
        {
            using (SqlConnection con = new SqlConnection(_hospitalConnectionString))
            {
                bool doesExist = await DoesPatientExist(Name, DateOfBirth);

                //Check against users based off Name and DoB, if this returns true, exception will be thrown saying that the Patient already exists
                if (doesExist == false)
                {

                    try
                    {
                        SqlCommand cmd = new SqlCommand("spHospital_AddPatient", con);
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Gender", Gender);
                        cmd.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                        cmd.CommandType = CommandType.StoredProcedure;

                        con.Open();

                        var result = await cmd.ExecuteNonQueryAsync();


                        if (result == 0)
                        {
                            throw new Exception("Unable to Add Patient");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        con.Close();
                    }
                }

                else
                {
                    throw new Exception("Patient Already Exists");
                }
            }
        }

        public async void AdmitPatient(Guid PatientID, int WardID, string Notes)
        {
            using (SqlConnection con = new SqlConnection(_hospitalConnectionString))
            {
                bool isPatientAdmitted = await IsPatientAdmitted(PatientID);

                try
                {
                    if (isPatientAdmitted == false)
                    {
                        SqlCommand cmd = new SqlCommand("spHospital_AdmitPatient", con);

                        cmd.Parameters.AddWithValue("@PatientID", PatientID);
                        cmd.Parameters.AddWithValue("@WardID", WardID);
                        cmd.Parameters.AddWithValue("@Notes", Notes);
                        cmd.Parameters.AddWithValue("@AdmitDate", DateTime.Now);
                        cmd.CommandType = CommandType.StoredProcedure;

                        con.Open();

                        var result = await cmd.ExecuteNonQueryAsync();

                        if (result == 0)
                        {
                            throw new Exception("Unable to Add Patient");
                        }
                    }
                    else
                    {
                        throw new Exception("Patient Already Admitted");
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public async void DischargePatient(Guid PatientID, Guid SpellID)
        {
            using (SqlConnection con = new SqlConnection(_hospitalConnectionString))
            {
                bool isPatientAdmitted = await IsPatientAdmitted(PatientID);

                try
                {
                    //If Patient is Admitted, then they can be discharged
                    if (isPatientAdmitted == true)
                    {
                        SqlCommand cmd = new SqlCommand("spHospital_DischargePatient", con);

                        cmd.Parameters.AddWithValue("@SpellID", SpellID);
                        cmd.Parameters.AddWithValue("@PatientID", PatientID);
                        cmd.Parameters.AddWithValue("@DischargeDate", DateTime.Now);
                        cmd.CommandType = CommandType.StoredProcedure;

                        con.Open();

                        var result = await cmd.ExecuteNonQueryAsync();

                        if (result == 0)
                        {
                            throw new Exception("Unable to Discharge Patient");
                        }
                    }
                    else
                    {
                        throw new Exception("Patient is not currently admitted");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public async Task<bool> DoesPatientExist(string Name, DateTime DateOfBirth)
        {
            //This method sends a potential users Name and Date Of Birth to the database, and returns a bit to determine if the 
            //patient already exisits in the database. This will be called when the Create Patient endpoint is called
            //the stored procedure contains the LOWER() flag on both the table name and the variable name, so the comparison will be
            //case insensitive. A check against name and DoB was chosen as people may share the same name, but not birthday

            using (SqlConnection con = new SqlConnection(_hospitalConnectionString))
            {
                bool result = false;
                try
                {
                    SqlCommand cmd = new SqlCommand("spHospital_DoesPatientExist", con);
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateOfBirth.ToString("yyyy/MM/dd"));
                    cmd.Parameters.Add("@Result", SqlDbType.Bit);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    result = Convert.ToBoolean(cmd.Parameters["@Result"].Value);

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    con.Close();
                }
                return result;
            }
        }

        public async Task<bool> IsPatientAdmitted(Guid PatientID)
        {
            bool result = false;
            using (SqlConnection con = new SqlConnection(_hospitalConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spHospital_IsPatientAdmitted", con);
                    cmd.Parameters.AddWithValue("@PatientID", PatientID);
                    cmd.Parameters.Add("@Result", SqlDbType.Bit);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    result = Convert.ToBoolean(cmd.Parameters["@Result"].Value);

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return result;
        }

        public async Task<List<PatientReadModel>> ListActivePatients()
        {
            List<PatientReadModel> patients = new List<PatientReadModel>();

            using (SqlConnection con = new SqlConnection(_hospitalConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spHospital_GetActivePatients", con);

                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        patients.Add(new PatientReadModel
                        {
                            PatientID = Guid.Parse(rdr["PatientID"].ToString()),
                            Name = rdr["Name"].ToString(),
                            DateOfBirth = Convert.ToDateTime(rdr["DateOfBirth"]),
                            Gender = rdr["Gender"].ToString(),
                            IsAdmitted = Convert.ToBoolean(rdr["IsAdmitted"])
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return patients;
        }


        public async Task<List<PatientWardReadModel>> ListActivePatientsByWard(int WardID)
        {
            List<PatientWardReadModel> patients = new();

            using (SqlConnection con = new SqlConnection(_hospitalConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spHospital_GetActivePatientsByWard", con);
                    cmd.Parameters.AddWithValue("@WardID", WardID);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        patients.Add(new PatientWardReadModel
                        {
                            PatientID = Guid.Parse(rdr["PatientID"].ToString()),
                            Name = rdr["Name"].ToString(),
                            DateOfBirth = Convert.ToDateTime(rdr["DateOfBirth"]),
                            Gender = rdr["Gender"].ToString(),
                            WardID = Convert.ToInt32(rdr["WardID"]),
                            WardName = rdr["WardName"].ToString(),
                            IsAdmitted = Convert.ToBoolean(rdr["IsAdmitted"])
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return patients;
        }
        public async void UpdatePatient(Guid PatientID, string Name, string Gender, DateTime? DateOfBirth)
        {
            using(SqlConnection con = new SqlConnection(_hospitalConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spHospital_UpdatePatient", con);

                    cmd.Parameters.AddWithValue("@PatientID", PatientID);
                    cmd.Parameters.AddWithValue("@Name", !String.IsNullOrWhiteSpace(Name) ? Name : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Gender", !String.IsNullOrWhiteSpace(Gender) ? Gender: DBNull.Value);
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateOfBirth != null ? DateOfBirth.Value.ToString("yyyy/MM/dd") : DBNull.Value);

                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();

                    var result = await cmd.ExecuteNonQueryAsync();

                    if(result == 0)
                    {
                        throw new Exception("Unable to update Patient");
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }                
            }
        }
    }
}
