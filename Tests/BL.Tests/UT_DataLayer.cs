using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLayer = BlueBit.HR.Docs.BL.DataLayer;

namespace BlueBit.HR.Docs.BL.Tests
{
    [TestClass]
    public class UT_DataLayer
    {
        [TestMethod]
        public void TestMethod_LoadData()
        {
            var sessionFactory = DataLayer.Cfg.Defines.CreateSessionFactory();
            using (var session = sessionFactory.OpenSession())
            {
                var documentsLoad = new DataLayer.Entities.DocumentsLoad();
                documentsLoad.DateStart = DateTime.Now;
                session.Save(documentsLoad);

                for (var year = 2012; year < 2014; ++year)
                    for (var month = 1; month < 13; ++month)
                        for (var i = 0; i < 10; ++i)
                        {
                            var document = new DataLayer.Entities.DocumentWithData();
                            document.DocumentsLoad = documentsLoad;
                            document.PESEL = "7306100537" + i.ToString();
                            document.DateYear = 2013;
                            document.DateMonth = 10;
                            document.Data = System.IO.File.ReadAllBytes(@"D:\ILF\KADRY\INTRANET\2012\RapTygKomen\Affek, Marcin\2013_27-tydzien zesp_Affek, Marcin_01.07-31.07.2013.xlsx");
                            session.Save(document);
                        }

                documentsLoad.DateStop = DateTime.Now;
                session.Update(documentsLoad);
            }
        }

        [TestMethod]
        public void TestMethod_CreateEmployees()
        {
            var sessionFactory = DataLayer.Cfg.Defines.CreateSessionFactory();
            using (var session = sessionFactory.OpenSession())
            {
                for (var i = 0; i < 10; ++i)
                {
                    var employee = new DataLayer.Entities.Employee();
                    employee.Identifier = "EMP_" + i.ToString();
                    employee.PIN = "000" + i.ToString();
                    employee.IsAdministrator = i == 0;
                    employee.PESEL = "7306100537" + i.ToString();
                    session.Save(employee);
                }
            }
        }
    }
}
