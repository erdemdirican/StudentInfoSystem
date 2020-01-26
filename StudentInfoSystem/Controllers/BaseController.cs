using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentInfoSystem.Database;

namespace StudentInfoSystem.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected StudentDbEntities context { get; private set; }
        public static string logonUserName { get; set; }
        public BaseController()
        {
            context = new StudentDbEntities();
        }
    }
}