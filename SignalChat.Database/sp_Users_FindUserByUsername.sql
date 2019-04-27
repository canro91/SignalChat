CREATE PROCEDURE [dbo].[sp_Users_FindUserByUsername]
	@Username VARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ID, Username, SaltedPassword FROM Users WHERE Username = @Username
END
