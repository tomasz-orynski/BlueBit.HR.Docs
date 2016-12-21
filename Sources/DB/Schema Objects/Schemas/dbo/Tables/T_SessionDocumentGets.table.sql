CREATE TABLE [dbo].[T_SessionDocumentGets]
(
	ID bigint NOT NULL IDENTITY(1,1),

	SessionID bigint NOT NULL,
	DocumentID bigint NOT NULL,
	Date datetime NOT NULL,
)
