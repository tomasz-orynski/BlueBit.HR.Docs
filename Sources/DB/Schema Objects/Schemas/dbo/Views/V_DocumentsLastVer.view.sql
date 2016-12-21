CREATE VIEW [dbo].[V_DocumentsLastVer]
AS SELECT * 
FROM [T_Documents] doc
WHERE doc.DocumentsLoadID = (SELECT MAX(DocumentsLoadID) FROM [T_Documents] docMax WHERE docMax.PESEL = doc.PESEL AND docMax.DateYear = doc.DateYear AND docMax.DateMonth = doc.DateMonth);
