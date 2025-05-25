using System.Web;
using System.Web.Mvc;
using LoanApplicationSystem.Models;
using System.IO;
using System.Linq;

namespace LoanApplicationSystem.Controllers
{
    public class LoanController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Apply(LoanApplication model, HttpPostedFileBase uploadDocument)
        {
            if (ModelState.IsValid)
            {
                if (uploadDocument != null && uploadDocument.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(uploadDocument.FileName);
                    var path = Path.Combine(Server.MapPath("~/UploadedDocs/"), fileName);
                    uploadDocument.SaveAs(path);
                    model.UploadedFileName = fileName;
                }

                db.LoanApplications.Add(model);
                db.SaveChanges();
                return RedirectToAction("Success");
            }
            return View(model);
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult AdminList()
        {
            return View(db.LoanApplications.ToList());
        }
        [HttpPost]
        public ActionResult UpdateStatus(int ApplicationId, ApplicationStatus Status)
        {
            var application = db.LoanApplications.FirstOrDefault(a => a.Id == ApplicationId);
            if (application != null)
            {
                application.Status = Status;
                db.SaveChanges();
                TempData["Message"] = "Status updated successfully!";
            }
            return RedirectToAction("AdminList");
        }

    }
}





@{
    var statusClass = item.Status == ApplicationStatus.Approved ? "bg-success text-white"
                    : item.Status == ApplicationStatus.Rejected ? "bg-danger text-white"
                    : "bg-warning text-dark";
}
<td><span class="badge @statusClass">@item.Status</span></td>





























using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanApplicationSystem.Models;

namespace LoanApplicationSystem.Controllers
{
    public class LoanController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Apply(LoanApplication model, HttpPostedFileBase uploadDocument)
        {
            if (ModelState.IsValid)
            {
                if (uploadDocument != null && uploadDocument.ContentLength > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadDocument.FileName);
                    var path = Path.Combine(Server.MapPath("~/UploadedDocs/"), fileName);
                    uploadDocument.SaveAs(path);
                    model.UploadedFileName = fileName;
                }

                model.SubmissionDate = DateTime.Now;
                db.LoanApplications.Add(model);
                db.SaveChanges();
                return RedirectToAction("Success");
            }

            return View(model);
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult AdminList()
        {
            return View(db.LoanApplications.ToList());
        }

        [HttpPost]
        public ActionResult UpdateStatus(int ApplicationId, ApplicationStatus Status)
        {
            var application = db.LoanApplications.FirstOrDefault(a => a.Id == ApplicationId);
            if (application != null)
            {
                application.Status = Status;
                db.SaveChanges();
                TempData["Message"] = "Status updated successfully!";
            }
            return RedirectToAction("AdminList");
        }
    }
}

