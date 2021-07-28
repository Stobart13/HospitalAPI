CREATE PROCEDURE [dbo].[spHospital_GetActivePatients]
AS
BEGIN
	SELECT * FROM Patients
	WHERE IsAdmitted =1
END
GO