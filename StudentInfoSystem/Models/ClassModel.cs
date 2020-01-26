using StudentInfoSystem.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentInfoSystem.Models
{
    public class ClassModel
    {
        public Classes class1 { get; set; }
        public int studentCounter { get; set; }

        //public int Id { get; set; }
        //public string ClassName { get; set; }
        //public List<StudentModel> Student { get; set; }
    }
}