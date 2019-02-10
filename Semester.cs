using System;
using System.Collections.Generic;

namespace SoftwareDesign
{
    class Semester
    {
        public string Name;
        public int Students;
        public List<Subject> Subjects = new List<Subject>();
        public string[] Availability = new string[50];
    }
}