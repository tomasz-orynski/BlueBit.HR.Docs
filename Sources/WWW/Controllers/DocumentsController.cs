using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using BL = BlueBit.HR.Docs.BL;
using BlueBit.HR.Docs.BL.Diagnostics;

namespace BlueBit.HR.Docs.WWW.Controllers
{
    public class DocumentsController : Code.CommonController<DocumentsController>
    {
        [Authorize]
        public ActionResult Index(string sortOrder, string currentSortOrder, int? page)
        {
            return _HandleUserRequest(() => {
                var logic = new Models.Documents.Logic();
                var documents = logic.GetDocuments();

                ViewBag.CurrentSort = sortOrder;
                var pageSize = 10;
                var pageNumber = page ?? 1;

                switch (sortOrder)
                {
                    case "Year":
                        if (sortOrder == currentSortOrder)
                            return View(documents.OrderByDescending(d => d.DateYear).ToPagedList(pageNumber, pageSize));
                        else
                            return View(documents.OrderBy(d => d.DateYear).ToPagedList(pageNumber, pageSize));

                    case "Month":
                        if (sortOrder == currentSortOrder)
                            return View(documents.OrderByDescending(d => d.DateMonth).ToPagedList(pageNumber, pageSize));
                        else
                            return View(documents.OrderBy(d => d.DateMonth).ToPagedList(pageNumber, pageSize));
                }

                return View(documents.OrderByDescending(d => d.DateYear * 100 + d.DateMonth).ToPagedList(pageNumber, pageSize));
            });
        }

        [Authorize]
        public ActionResult Get(long id)
        {
            return _HandleUserRequest(() => {
                var logic = new Models.Documents.Logic();
                var data = logic.GetDocumentData(id);
                return File(data.Item1, data.Item2, data.Item3);
            });
        }
    }
}
