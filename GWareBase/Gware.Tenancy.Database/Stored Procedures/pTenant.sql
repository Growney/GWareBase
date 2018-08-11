CREATE PROCEDURE [dbo].[pTenant]
	@Result NCHAR(50),
	@ID BIGINT = NULL,
	@Name VARCHAR(100) = NULL,
	@CreatedDate DATETIME = NULL,
	@ControllerCreationString  VARCHAR(MAX) = NULL,
	@DisplayName VARCHAR(100) = NULL,
	@EntityType INT = NULL,
	@EntityID BIGINT = NULL,
	@UpgradeCheck DATETIME = NULL,
	@UpgradeStatus TINYINT = NULL,
	@CheckAgainst DATETIME = NULL
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

		INSERT INTO Tenant (Name,ControllerCreationString,DisplayName,EntityType,EntityID,UpgradeCheck,UpgradeStatus)
		VALUES(@Name,@ControllerCreationString,@Displayname,@EntityType,@EntityID,@UpgradeCheck,@UpgradeStatus)

		SELECT @@IDENTITY AS [Value]

	END
	ELSE
	BEGIN

		UPDATE Tenant
		SET DisplayName = @DisplayName
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

IF @Result = 'ForEntity'
BEGIN

	SELECT*
	FROM Tenant
	WHERE EntityID = @EntityID
	AND EntityType = @EntityType

END

IF @Result = 'EntityExists'
BEGIN

	SELECT CASE WHEN (SELECT COUNT(*) FROM Tenant WHERE EntityID = @EntityID AND EntityType = @EntityType) > 0 THEN 1 ELSE 0 END AS [Value]
	
END

IF @Result = 'CheckUpgradeStatus'
BEGIN

	BEGIN TRANSACTION
		
		SELECT CASE WHEN UpgradeStatus = 0 AND [UpgradeCheck] < @CheckAgainst THEN 1 ELSE UpgradeStatus END AS [Value] 
		FROM Tenant
		WHERE Id = @ID
		
	COMMIT
END

IF @Result = 'SetCheckDate'
BEGIN

	UPDATE Tenant
	SET UpgradeCheck = GETUTCDATE(),
	UpgradeStatus = @UpgradeStatus
	WHERE Id = @Id

END

IF @Result = 'SetUpgradeStatus'
BEGIN
	
	BEGIN TRANSACTION

	UPDATE Tenant
	SET UpgradeStatus = @UpgradeStatus
	WHERE Id = @ID

	SELECT UpgradeStatus AS [Value]
	FROM Tenant
	WHERE Id = @Id

	COMMIT

END
END