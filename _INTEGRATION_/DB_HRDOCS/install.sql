-- usuwanie obiektów jeœli ju¿ by³y...

DROP SYNONYM S_HRDOCS_INFO
GO

-- tworzenie obiektów

CREATE SYNONYM S_HRDOCS_INFO
FOR HRSYS.dbo.V_HRDOCS_INFO -- HRSYS to nazwa bazy systemu HR-owego
GO
