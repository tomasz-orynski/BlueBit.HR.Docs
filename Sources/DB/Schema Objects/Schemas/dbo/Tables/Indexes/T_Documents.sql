CREATE INDEX IX_Documents_01 ON T_Documents(PESEL, DateYear, DateMonth, DocumentsLoadID);
GO
CREATE INDEX IX_Documents_02 ON T_Documents(DocumentsLoadID);
GO
