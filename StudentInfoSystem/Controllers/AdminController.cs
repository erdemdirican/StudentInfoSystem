using StudentInfoSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;


namespace StudentInfoSystem.Controllers
{
    public class AdminController : BaseController
    {
        // GET: Admin
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(Models.LoginModel model)
        {
            try
            {
                var admin = context.Logins.FirstOrDefault(x => x.Username == model.admin.Username && x.Password == model.admin.Password);
                if (admin != null)
                {
                    Session["LogonAdmin"] = admin;
                    logonUserName = admin.Username;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Admin information is incorrect!";
                    return View();
                }
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session["LogonAdmin"] = null;
            return RedirectToAction("Login", "Admin");
        }

        [HttpGet]
        public ActionResult Profil(int id)
        {
            StudentModel student = new StudentModel();
            student.student1 = context.Students.FirstOrDefault(x => x.Id == id);
            student.className = context.Classes.FirstOrDefault(x => x.Id == student.student1.ClassId).ClassName;

            return View(student);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (Session["LogonAdmin"] != null)
            {
                Models.ChangePassword model = new Models.ChangePassword();

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(Models.ChangePassword model)
        {

            if (Session["LogonAdmin"] == null)
            {
                TempData["resultInfo"] = "Your session has expired. Please Login and Try Again!";
                return RedirectToAction("Login", "Action");
            }
            if (model.admin.Password == model.password)
            {
                var admin = context.Logins.FirstOrDefault(x => x.Username == logonUserName);
                if (admin != null)
                {
                    if (admin.Password == model.currentPassword)
                    {
                        admin.Password = model.admin.Password;
                        try
                        {
                            context.Entry<Database.Logins>(admin).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                            TempData["resultInfo"] = "Your Password has been changed successfully";
                        }
                        catch (Exception ex)
                        {
                            TempData["resultInfo"] = "Sorry, your password could not be changed!";
                        }
                        return View(new Models.ChangePassword());
                    }
                    else
                    {
                        TempData["resultInfo"] = "Your current password is not correct";
                        return View(new Models.ChangePassword());
                    }
                }
                else
                {
                    TempData["resultInfo"] = "Your password could not be changed!";
                    return View(new Models.ChangePassword());
                }
            }
            else
            {
                TempData["resultInfo"] = "Passwords do not match!";
                return View(new Models.ChangePassword());
            }

        }

        [HttpGet]
        public ActionResult ProfileCreate()
        {
            if (ModelState.IsValid)
            {
                Database.Students model = new Database.Students();
                var classes = context.Classes.Select(x => new SelectListItem()
                {
                    Text = x.ClassName,
                    Value = x.Id.ToString()
                }).ToList();
                ViewBag.classList = classes;

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public ActionResult ProfileCreate(Database.Students student)
        {
            System.Diagnostics.Debug.WriteLine("SomeText");
            System.Diagnostics.Debug.WriteLine("Student Number" + student.StudentNo);
            System.Diagnostics.Debug.WriteLine("Name" + student.StudentName);
            System.Diagnostics.Debug.WriteLine("Surname" + student.StudentSurname);
            System.Diagnostics.Debug.WriteLine("Class" + student.ClassId);

            if (ModelState.IsValid)
            {
                context.Entry<Database.Students>(student).State = System.Data.Entity.EntityState.Added;
                try
                {
                    context.SaveChanges();
                    TempData["resultInfo"] = "New profile has been created.";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    TempData["resultInfo"] = ex.Message;
                    var classes = context.Classes.Select(x => new SelectListItem()
                    {
                        Text = x.ClassName,
                        Value = x.Id.ToString()
                    }).ToList();
                    ViewBag.classList = classes;

                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["resultInfo"] = "New profile has not been created...Please check your definitions";

                return RedirectToAction("Index", "Home");
            }


        }

        [HttpGet]
        public ActionResult ProfileEdit(int id = 0)
        {
            var student = new Database.Students();
            student.Id = 0;
            Database.Students model = new Database.Students();
            if (id != 0)
            {
                student = context.Students.FirstOrDefault(x => x.Id == id);
                model = student;
            }
            System.Diagnostics.Debug.WriteLine("aaa--" + model.Id + "--");

            var classes = context.Classes.Select(x => new SelectListItem()
            {
                Text = x.ClassName,
                Value = x.Id.ToString()
            }).ToList();
            ViewBag.classList = classes;

            return View(model);
        }

        [HttpPost]
        public ActionResult ProfileEdit(Database.Students model)
        {
            context.Entry<Database.Students>(model).State = System.Data.Entity.EntityState.Modified;
            try
            {
                context.SaveChanges();
                TempData["resultInfo"] = "Information updated!";
                return RedirectToAction("Profil", "Admin", new { id = model.Id });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("aaa--" + "" + ex.Message);
                TempData["resultInfo"] = "The information could not be updated!";
                var user = context.Students.FirstOrDefault(x => x.Id == model.Id);
                StudentModel student = new StudentModel();
                student.student1 = user;
                context.Entry<Database.Students>(model).State = System.Data.Entity.EntityState.Added;
                return RedirectToAction("ProfileEdit", "Admin", new { id = model.Id });
            }
        }

        public ActionResult Delete(int id)
        {
            var student = context.Students.FirstOrDefault(x => x.Id == id);

            context.Entry<Database.Students>(student).State = System.Data.Entity.EntityState.Deleted;
            try
            {
                context.SaveChanges();
                TempData["resultInfo"] = "Student has been deleted!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["resultInfo"] = "The student could not be deleted for some reason!";
                return RedirectToAction("Index", "Home");
            }

        }

    }
}