ALTER TABLE T_SessionDocumentGets ADD CONSTRAINT PK_SessionDocumentGets PRIMARY KEY CLUSTERED (ID);
GO
ALTER TABLE T_SessionDocumentGets ADD CONSTRAINT FK_SessionDocumentGets_01 FOREIGN KEY (SessionID) REFERENCES T_Sessions(ID);
GO
ALTER TABLE T_SessionDocumentGets ADD CONSTRAINT FK_SessionDocumentGets_02 FOREIGN KEY (DocumentID) REFERENCES T_Documents(ID);
GO