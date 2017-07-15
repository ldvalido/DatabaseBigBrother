BEGIN TRY
	DROP TRIGGER trBigBrother ON Database
END TRY
BEGIN CATCH
END CATCH
GO

CREATE TRIGGER trBigBrother ON DATABASE 
FOR
	Create_PROCEDURE,ALTER_PROCEDURE
AS
	--Transparent interceptor
	SET NOCOUNT ON
	DECLARE @query XML
	DECLARE @r int
	SELECT @query = EventData() 
	exec spBigBrother @query,@r out
	if (@r <> 0) BEGIN
		--BLA BLA
		ROLLBACK	
	END
	SET NOCOUNT OFF
DISABLE TRIGGER trBigBrother ON DATABASE 
GO
ENABLE TRIGGER trBigBrother ON DATABASE
GO

ALTER PROCEDURE [dbo].[t]
AS	
/**/
--
	SELECT [eventData] FROM Audits
	
INSERT INTO Employees VALUES ('topo')	
GO