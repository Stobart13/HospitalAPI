CREATE PROCEDURE [dbo].[spHospital_DischargePatient]
(
	@SpellID uniqueidentifier,
	@PatientID uniqueidentifier,
	@DischargeDate datetime
)
AS
BEGIN
	UPDATE Spells
	SET DischargeDate = @DischargeDate
	Where SpellID = @SpellID

	UPDATE Patients
	SET IsAdmitted = 0
	WHERE PatientID = @PatientID
END
GO
