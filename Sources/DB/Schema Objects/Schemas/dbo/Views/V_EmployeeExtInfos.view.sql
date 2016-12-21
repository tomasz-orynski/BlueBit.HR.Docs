CREATE VIEW [dbo].[V_EmployeeExtInfos]
AS SELECT
	emp.ID,
	empi.PESEL,
	empi.NIP,
	empi.DOW_OSOB_DATA_WAZ AS IdentCardExpireDate,
	empi.PASZPORT_DATA_WAZ AS PassportExpireDate,
	empi.URZAD_SKARB AS TaxOffice,
	empi.TELEFON AS PhoneNo,
	empi.RACHUNEK_BANK AS BankAccount,
	empi.ADRESM_MIASTO AS AddressCity,
	empi.ADRESM_ULICA AS AddressStreet,
	empi.ADRESM_NR_DOMU AS AddressHouseNo,
	empi.ADRESM_NR_MIESZ AS AddressApartNo,
	empi.ADRESZ_MIASTO AS AddressRegCity,
	empi.ADRESZ_ULICA AS AddressRegStreet,
	empi.ADRESZ_NR_DOMU AS AddressRegHouseNo,
	empi.ADRESZ_NR_MIESZ AS AddressRegApartNo,
	empi.URLOP_ZALEGLY As LeavePrev,
	empi.URLOP_NALEZNY As LeaveOwing,
	empi.URLOP_WYKORZYSTANY As LeaveUse,
	empi.URLOP_POZOSTALY As LeaveRemaining,
	empi.URLOP_OPIEK As LeaveCare,
	empi.BADANIA_DATA_WAZ AS ExamExpireDate
FROM [T_Employees] emp
LEFT JOIN [S_HRDOCS_INFO] empi ON emp.PESEL = empi.PESEL COLLATE Polish_CI_AS --SQL_Latin1_General_CP1_CI_AS
