-- usuwanie obiekt�w je�li ju� by�y...

DROP SYNONYM S_HRDOCS_INFO
GO

-- tworzenie obiekt�w

CREATE SYNONYM S_HRDOCS_INFO
FOR HRSYS.dbo.V_HRDOCS_INFO -- HRSYS to nazwa bazy systemu HR-owego
GO
