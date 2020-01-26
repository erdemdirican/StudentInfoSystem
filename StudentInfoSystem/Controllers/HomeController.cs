using StudentInfoSystem.Database;
using StudentInfoSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;


namespace StudentInfoSystem.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            List<Students> students = context.Students.Where(x => x.Id != 0).ToList();

            return View(students);
        }

        [HttpGet]
        public ActionResult ClassControl()
        {
            List<ClassModel> classListModel = new List<ClassModel>();
            List<Database.Classes> classList = context.Classes.Where(x => x.Id != 0).ToList();
            System.Diagnostics.Debug.WriteLine("aa--" + classList.Count + "--");
            List<int> studentCounter = new List<int>();
            for (int i = 0; i < classList.Count; i++)
            {
                ClassModel model = new ClassModel();
                model.class1 = classList[i];
                System.Diagnostics.Debug.WriteLine("aa--" + model.class1 + "--");
                System.Diagnostics.Debug.WriteLine("aa--" + classList[i] + "--");
                System.Diagnostics.Debug.WriteLine("aa--" + i + "--");
                model.studentCounter = Convert.ToInt32(context.Students.Where(x => x.ClassId == model.class1.Id).ToList().Count());
                classListModel.Add(model);
            }
            return View(classListModel);
        }

        public ActionResult Delete(int id = 0)
        {
            if (id == 0)
            {
                TempData["resultInfo"] = "It could not be deleted because there are existing students in the class.";
                return RedirectToAction("ClassControl", "Home");
            }
            var class1 = context.Classes.FirstOrDefault(x => x.Id == id);
            string name = class1.ClassName;
            context.Entry<Database.Classes>(class1).State = System.Data.Entity.EntityState.Deleted;
            try
            {
                context.SaveChanges();
                TempData["resultInfo"] = name + " class has been deleted.";
                return RedirectToAction("ClassControl", "Home");
            }
            catch (Exception ex)
            {
                TempData["resultInfo"] = "Deletion failed.";
                return RedirectToAction("ClassControl", "Home");
            }

        }

        [HttpGet]
        public ActionResult ClassCreate()
        {
            if (ModelState.IsValid)
            {
                if (Session["LogonAdmin"] != null)
                {
                    Classes model = new Classes();

                    return View(model);
                }
                else
                {
                    return RedirectToAction("ClassControl", "Home");
                }
            }
            else
            {
                return RedirectToAction("ClassControl", "Home");
            }
        }

        [HttpPost]
        public ActionResult ClassCreate(Classes model)
        {
            System.Diagnostics.Debug.WriteLine("aa--" + model.ClassName + "--");

            if (ModelState.IsValid)
            {
                if (Session["LogonAdmin"] == null)
                {
                    TempData["resultInfo"] = "Your session has expired. Please Login and Try Again!";
                    return RedirectToAction("Login", "Action");
                }

                if (model.ClassName != "")
                {
                    if (context.Classes.Any(x => x.ClassName == model.ClassName))
                    {
                        TempData["resultInfo"] = "There is a class which is the same name!";
                        return View();
                    }
                    try
                    {

                        context.Entry<Database.Classes>(model).State = System.Data.Entity.EntityState.Added;
                        TempData["resultInfo"] = model.ClassName + " class has been added.";
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        TempData["resultInfo"] = ex.Message;
                    }
                    return RedirectToAction("ClassControl", "Home");
                }
                else
                {
                    TempData["resultInfo"] = "Enter Class Name!";
                    return RedirectToAction("ClassControl", "Home");
                }

            }
            else
            {
                TempData["resultInfo"] = "New profile has not been created...Please check your definitions";

                return RedirectToAction("ClassControl", "Home");
            }

        }

    }
}