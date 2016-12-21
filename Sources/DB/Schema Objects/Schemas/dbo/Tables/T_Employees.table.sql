CREATE TABLE [dbo].[T_Employees]
(
	ID bigint NOT NULL IDENTITY(1,1),
    PESEL nvarchar(11) NOT NULL,

    Identifier nvarchar(100) NOT NULL,
    PIN nvarchar(10) NOT NULL,
    IsAdministrator bit NOT NULL,
	IsLogonMailSend bit NOT NULL,
)
