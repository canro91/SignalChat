CREATE PROCEDURE dbo.sp_Messages_Save
	@ID UNIQUEIDENTIFIER,
	@Username VARCHAR(256),
	@Body VARCHAR(2048),
	@DeliveredAt DATETIMEOFFSET
AS
BEGIN
		SET NOCOUNT ON;

		INSERT INTO Messages
				   (ID
				   ,Username
				   ,Body
				   ,DeliveredAt)
			 VALUES
				   (@ID
				   ,@Username
				   ,@Body
				   ,@DeliveredAt)
END
