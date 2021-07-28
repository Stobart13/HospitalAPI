CREATE PROCEDURE [dbo].[spHospital_AddPatient]
(
	@Name nvarchar(100),
	@Gender nvarchar(1),
	@DateOfBirth datetime
)
AS
BEGIN
	INSERT INTO Patients(Name,Gender,DateOfBirth,IsAdmitted)
	SELECT @Name,@Gender,@DateOfBirth,0
END
GO