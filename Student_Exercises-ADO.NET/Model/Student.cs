using System;
using System.Collections.Generic;
using System.Text;

namespace Student_Exercises_ADO.NET.Model
{
    class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SlackHandle { get; set; }
        public int CohortId { get; set; }
    }
}
