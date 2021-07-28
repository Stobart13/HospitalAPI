CREATE PROCEDURE [dbo].[spHospital_DoesPatientExist] 
(
	@Name nvarchar(100),
	@DateOfBirth datetime,
	@Result bit output
)
AS

BEGIN

DECLARE @PatientCount int

	SELECT @PatientCount = COUNT(PatientID)
	FROM Patients
	WHERE LOWER(Name) = LOWER(@Name) and DateOfBirth = @DateOfBirth

	IF @PatientCount > 0
	SET @Result =  1
	ELSE SET @Result = 0
END
GO