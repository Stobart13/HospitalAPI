CREATE PROCEDURE [dbo].[spHospital_UpdatePatient]
(
	@PatientID uniqueidentifier,
	@Name nvarchar(100),
	@Gender nvarchar(1),
	@DateOfBirth datetime
)
AS
BEGIN
	UPDATE Patients
	SET Name = ISNULL(@Name,Name),
	Gender = ISNULL(@Gender,Gender),
	DateOfBirth = ISNULL(@DateOfBirth, DateOfBirth)
	WHERE PatientID = @PatientID
END
GO