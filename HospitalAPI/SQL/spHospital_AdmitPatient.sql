CREATE PROCEDURE [dbo].[spHospital_AdmitPatient]
(
	@PatientID uniqueidentifier,
	@WardID int,
	@Notes nvarchar(100),
	@AdmitDate datetime
)
AS
BEGIN
	INSERT INTO Spells(AdmitDate,Notes,WardID,PatientID)
	SELECT @AdmitDate,@Notes,@WardID,@PatientID

	UPDATE Patients
	SET IsAdmitted = 1
	WHERE PatientID = @PatientID
END
GO