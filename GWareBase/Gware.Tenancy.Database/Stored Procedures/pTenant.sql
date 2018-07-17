CREATE PROCEDURE [dbo].[pTenant]
	@Result NCHAR(50),
	@ID BIGINT = NULL,
	@Name VARCHAR(100) = NULL,
	@CreatedDate DATETIME = NULL,
	@ControllerCreationString  VARCHAR(MAX) = NULL,
	@DisplayName VARCHAR(100) = NULL,
	@ImageSource VARCHAR(MAX) = NULL

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

		INSERT INTO Tenant (Name,ControllerCreationString,DisplayName,ImageSource)
		VALUES(@Name,@ControllerCreationString,@Displayname,@ImageSource)

		SELECT @@IDENTITY AS [Value]

	END
	ELSE
	BEGIN

		UPDATE Tenant
		SET DisplayName = @DisplayName,
		ImageSource = @ImageSource
		WHERE [Name] = @Name

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