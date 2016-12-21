CREATE TABLE [dbo].[T_Documents]
(
	ID bigint NOT NULL IDENTITY(1,1), 

    PESEL nvarchar(11) NOT NULL,
	DateYear int NOT NULL,
	DateMonth smallint NOT NULL,
	DocumentsLoadID bigint NOT NULL,

	Data varbinary(max) NOT NULL, 
)
