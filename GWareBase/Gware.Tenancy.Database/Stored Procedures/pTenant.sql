CREATE PROCEDURE [dbo].[pTenant]
	@Result NCHAR(50),
	@ID BIGINT = NULL,
	@Name VARCHAR(100) = NULL,
	@CreatedDate DATETIME = NULL,
	@ControllerCreationString  VARCHAR(MAX) = NULL

AS
BEGIN

IF @Result = 'Single'
BEGIN

	SELECT*
	FROM Tenant
	WHERE Id = @ID

END

IF @Result = 'Save'
BEGIN
	
	IF NOT EXISTS (SELECT* FROM Tenant WHERE Name = @Name)
	BEGIN

		INSERT INTO Tenant (Name,ControllerCreationString)
		VALUES(@Name,@ControllerCreationString)

		SELECT @@IDENTITY AS [Value]

	END
	ELSE
	BEGIN

		SELECT -1 AS [Value]

	END
	

END

IF @Result = 'Exists'
BEGIN

	SELECT CASE WHEN (SELECT COUNT(*) FROM Tenant WHERE Name = @Name) > 0 THEN 1 ELSE 0 END AS [Value]

END

IF @Result = 'Delete'
BEGIN

	DELETE FROM Tenant 
	WHERE Id = @ID

END

IF @Result = 'ForName'
BEGIN

	SELECT*
	FROM Tenant
	WHERE Name = @Name

END
END