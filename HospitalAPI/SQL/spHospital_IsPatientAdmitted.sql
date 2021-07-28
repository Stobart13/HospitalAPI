CREATE PROCEDURE [dbo].[spHospital_IsPatientAdmitted] 
(
	@PatientID uniqueidentifier,
	@Result bit output
)
AS

BEGIN
	DECLARE @IsAdmitted bit

	SELECT @IsAdmitted = IsAdmitted
	FROM Patients
	WHERE PatientID = @PatientID

	IF @IsAdmitted = 1
	SET @Result =  1
	ELSE SET @Result = 0
END
GO