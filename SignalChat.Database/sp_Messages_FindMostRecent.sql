CREATE PROCEDURE [dbo].[sp_Messages_FindMostRecent]
	@Count int = 50
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP(@Count)  Username, Body, DeliveredAt FROM Messages
	ORDER BY DeliveredAt
END
