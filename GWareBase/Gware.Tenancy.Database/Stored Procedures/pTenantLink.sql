CREATE PROCEDURE [dbo].[pTenantLink]
	@Result NCHAR(50),
	@ID BIGINT = NULL,
	@TypeID TINYINT = NULL,
	@TenantID BIGINT = NULL,
	@Link VARCHAR(MAX) = NULL
AS
IF @Result = 'Link'
BEGIN

	IF NOT EXISTS (SELECT * FROM TenantLink WHERE TenantID = @TenantID AND TypeID = @TypeID)
	BEGIN

		INSERT INTO TenantLink (TypeID,TenantID,Link)
		VALUES (@TypeID,@TenantID,@Link)

	END
	ELSE
	BEGIN

		UPDATE TenantLink
		SET Link = @Link
		WHERE TenantID = @TenantID AND TypeID = @TypeID 

	END

END

IF @Result = 'Remove'
BEGIN

	DELETE FROM TenantLink
	WHERE TenantID = @TenantID AND TypeID = @TypeID

END

IF @Result = 'SelectTenant'
BEGIN

	SELECT T.*,TL.Link,TL.TypeID
	FROM Tenant T
	INNER JOIN TenantLink TL ON T.Id = TL.TenantID
	WHERE TL.Link = @Link

END

IF @Result = 'GetLink'
BEGIN
	
	SELECT Link AS [Value]
	FROM TenantLink
	WHERE TenantID = @TenantID AND TypeID = @TypeID

END

IF @Result = 'All'
BEGIN

	SELECT T.*,TL.Link,TL.TypeID
	FROM Tenant T
	INNER JOIN TenantLink TL ON T.Id = TL.TenantID
	WHERE TL.TypeID = @TypeID

END