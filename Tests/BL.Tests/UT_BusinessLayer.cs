using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlueBit.HR.Docs.BL.BusinessLayer.Extensions;

namespace BlueBit.HR.Docs.BL.Tests
{
    [TestClass]
    public class UT_BusinessLayer :
        UT_Base
    {
        [TestMethod]
        public void TestMethod_LoadZipService()
        {
            var zipPath = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(typeof(UT_BusinessLayer).Assembly.Location), 
                    @"..\..\..\_Data_\Data.zip"));

            using (var businessCtx = new BusinessLayer.BusinessContext(e => e.Identifier == Consts.EmployeeIdentifier && e.PIN == Consts.EmployeePIN))
                businessCtx.DoInTransaction(transaction => {

                    using (var service = 
                        new BusinessLayer.Services.DataLoad.LoadZipService<BusinessLayer.Services.DataLoad.LoadZipServiceContext>(
                            new BusinessLayer.Services.DataLoad.LoadZipServiceContext(transaction, zipPath)
                        ))
                    {
                        service.Execute();
                    }
                });
        }

        [TestMethod]
        public void TestMethod_GetUsersService()
        {
            using (var businessCtx = new BusinessLayer.BusinessContext(e => e.Identifier == Consts.EmployeeIdentifier && e.PIN == Consts.EmployeePIN))
                businessCtx.DoInTransaction(transaction =>
                {
                    using (var service =
                        new BusinessLayer.Services.Enviroment.GetUsersService<BusinessLayer.Services.Enviroment.GetUsersServiceContext>(
                            new BusinessLayer.Services.Enviroment.GetUsersServiceContext(transaction)
                        ))
                    {
                        var result = service.Execute();
                        foreach (var r in result)
                        {
                            var s = r.FullName;
                        }
                    }
                });
        }
    }
}
