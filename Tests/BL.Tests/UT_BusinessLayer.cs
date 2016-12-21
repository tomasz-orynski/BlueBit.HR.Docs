using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLayer = BlueBit.HR.Docs.BL.DataLayer;
using BusinessLayer = BlueBit.HR.Docs.BL.BusinessLayer;
using BlueBit.HR.Docs.BL.BusinessLayer.Extensions;

namespace BlueBit.HR.Docs.BL.Tests
{
    [TestClass]
    public class UT_BusinessLayer
    {
        [TestMethod]
        public void TestMethod_LoadZipService()
        {
            var zipPath = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(typeof(UT_BusinessLayer).Assembly.Location), 
                    @"..\..\..\_Data_\Data.zip"));

            using (var businessCtx = new BusinessLayer.BusinessContext(e => e.Identifier == @"WAR-VTORYNSKI\Tomasz" && e.PIN == "1234"))
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
            using (var businessCtx = new BusinessLayer.BusinessContext(e => e.Identifier == @"WAR-VTORYNSKI\Tomasz" && e.PIN == "1234"))
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
