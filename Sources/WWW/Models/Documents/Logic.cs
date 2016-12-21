using System;
using System.Collections.Generic;
using BlueBit.HR.Docs.BL.BusinessLayer.Extensions;

namespace BlueBit.HR.Docs.WWW.Models.Documents
{
    public class Logic :
        BusinessLogicBase
    {
        public BL.DataLayer.Entities.EmployeeExtInfo GetEmployeeExtInfo()
        {
            var session = BusinessContext.OpenDBSession();
            return session.QueryOver<BL.DataLayer.Entities.EmployeeExtInfo>().Where(_ => _.ID == BusinessContext.Session.Employee.ID).SingleOrDefault();
        }


        public IList<BL.DataLayer.Entities.DocumentWithoutDataAndLastVer> GetDocuments()
        {
            var session = BusinessContext.OpenDBSession();
            return session.QueryOver<BL.DataLayer.Entities.DocumentWithoutDataAndLastVer>().Where(d => d.PESEL == BusinessContext.Session.Employee.PESEL).List();
        }

        public BL.DataLayer.Entities.DocumentWithData GetDocument(long id)
        {
            return BusinessContext.DoInTransactionWithResult(transaction =>
            {
                var session = BusinessContext.Session;
                var document = transaction.DBSession.QueryOver<BL.DataLayer.Entities.DocumentWithData>().Where(d => d.ID == id && d.PESEL == session.Employee.PESEL).SingleOrDefault();
                var sessionDocGet = new BL.DataLayer.Entities.SessionDocumentGet() { Date = DateTime.Now, Session = session, Document = document };
                transaction.DBSession.Insert(sessionDocGet);
                return document;
            });
        }

        public Tuple<byte[], string, string> GetDocumentData(long id)
        {
            var document = GetDocument(id);
            var fileName = string.Format("HRDOC_{0}_{1}_v{2}.pdf", document.PESEL, document.DateYear*100 + document.DateMonth, document.DocumentsLoad.ID);
            return Tuple.Create(document.Data, "application/pdf", fileName);
        }
    }
}