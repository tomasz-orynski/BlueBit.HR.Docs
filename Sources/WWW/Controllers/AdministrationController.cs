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
    public class AdministrationController : Code.CommonController<AdministrationController>
    {
        [Authorize]
        public ActionResult Index()
        {
            return _HandleUserRequest(() => {
                return View();
            });
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangePIN()
        {
            return _HandleUserRequest(() => {
                    var model = new Models.Administration.ChangePIN() { IsLogonMailSend = WWW.MvcApplication.GetAppInstance().BusinessContext.Session.Employee.IsLogonMailSend };
                    return View("ChangePIN", model);
                });
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangePIN(Models.Administration.ChangePIN model)
        {
            return _HandleUserRequest(() => {
                if (!ModelState.Values.SelectMany(v => v.Errors).Any())
                {
                    var m = new Models.Administration.Logic();
                    m.ChangePIN(model.NewPIN, model.IsLogonMailSend);
                    return View("~/Views/Shared/Success.cshtml");
                }
                else return View(model);
            });
        }

        [Authorize]
        public ActionResult EditUsers(string sortOrder, string currentSortOrder, int? page)
        {
            return _HandleAdminRequest(() => {
                var logic = new Models.Administration.Logic();
                var users = logic.GetUsers();

                ViewBag.CurrentSort = sortOrder;
                var pageSize = 10;
                var pageNumber = page ?? 1;

                switch (sortOrder)
                {
                    case "Identifier":
                        if (sortOrder == currentSortOrder)
                            return View(users.OrderByDescending(u => u.Identifier).ToPagedList(pageNumber, pageSize));
                        else
                            return View(users.OrderBy(u => u.Identifier).ToPagedList(pageNumber, pageSize));

                    case "FullName":
                        if (sortOrder == currentSortOrder)
                            return View(users.OrderByDescending(u => u.FullName).ToPagedList(pageNumber, pageSize));
                        else
                            return View(users.OrderBy(u => u.FullName).ToPagedList(pageNumber, pageSize));

                    case "PESEL":
                        if (sortOrder == currentSortOrder)
                            return View(users.OrderByDescending(u => u.PESEL).ToPagedList(pageNumber, pageSize));
                        else
                            return View(users.OrderBy(u => u.PESEL).ToPagedList(pageNumber, pageSize));

                    case "IsAdministrator":
                        if (sortOrder == currentSortOrder)
                            return View(users.OrderByDescending(u => u.IsAdministrator).ToPagedList(pageNumber, pageSize));
                        else
                            return View(users.OrderBy(u => u.IsAdministrator).ToPagedList(pageNumber, pageSize));
                }

                return View(users.OrderByDescending(u => u.Identifier).ToPagedList(pageNumber, pageSize));
            });
        }

        [HttpGet]
        [Authorize]
        public ActionResult CreateUser(string identifier)
        {
            return _HandleAdminRequest(() => {
                var model = new Models.Administration.UserData() { Identifier = identifier };
                return View("EditUser", model);
            });
        }

        [HttpGet]
        [Authorize]
        public ActionResult EditUser(long id)
        {
            return _HandleAdminRequest(() => {
                var logic = new Models.Administration.Logic();
                var entity = logic.GetEntity<BL.DataLayer.Entities.Employee>(employee => employee.ID == id);
                var model = new Models.Administration.UserData(entity);
                return View("EditUser", model);
            });
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditUser(Models.Administration.UserData model)
        {
            return _HandleAdminRequest(() => {
                if (!ModelState.Values.SelectMany(v => v.Errors).Any())
                {
                    var logic = new Models.Administration.Logic();
                    if (logic.SaveEntityModel(model, (t, m) => {
                        var errors = logic.CheckUser(t, m).ToList();
                        foreach (var e in errors) ModelState.AddModelError(e.Item1, e.Item2);
                        return errors.Count == 0;
                    }))
                        return View("~/Views/Shared/Success.cshtml");
                }
                return View(model);
            });
        }

        [HttpGet]
        [Authorize]
        public ActionResult LoadData()
        {
            return _HandleAdminRequest(() => {
                return View("LoadData");
            });
        }

        [HttpPost]
        [Authorize]
        public ActionResult LoadData(HttpPostedFileBase file)
        {
            return _HandleAdminRequest(() => {
                var m = new Models.Administration.Logic();
                m.LoadData(file.InputStream);
                return View("~/Views/Shared/Success.cshtml");
            });
        }
    }
}
