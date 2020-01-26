using StudentInfoSystem.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentInfoSystem.Models
{
    public class StudentModel
    {
        //public int Id { get; set; }
        public Students student1 { get; set; }
        public string className { get; set; }
        //public string Name { get; set; }
        //public int ClassId { get; set; }
        public ClassModel Class { get; set; }
    }
}