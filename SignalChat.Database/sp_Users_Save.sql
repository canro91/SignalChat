CREATE PROCEDURE [dbo].[sp_Users_Save]
	@ID UNIQUEIDENTIFIER,
	@Username VARCHAR(256),
	@SaltedPassword VARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.Users(ID, Username, SaltedPassword)
	VALUES (@ID, @Username, @SaltedPassword)
END
