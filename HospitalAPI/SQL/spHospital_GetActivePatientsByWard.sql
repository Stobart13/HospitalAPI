CREATE PROCEDURE [dbo].[spHospital_GetActivePatientsByWard]
(
	@WardID int
)
AS
BEGIN
	SELECT p.*, w.Name as WardName FROM Patients p
	INNER JOIN Spells s on s.PatientID = p.PatientID
	INNER JOIN Wards w on w.WardID = s.WardID and w.WardID = @WardID
	WHERE IsAdmitted =1
END
GO