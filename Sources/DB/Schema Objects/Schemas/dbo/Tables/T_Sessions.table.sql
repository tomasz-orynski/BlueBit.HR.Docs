CREATE TABLE [dbo].[T_Sessions]
(
	ID bigint NOT NULL IDENTITY(1,1),

	EmployeeID bigint NOT NULL,
	DateStart datetime NOT NULL,
	DateStop datetime NULL,
)
