using System;
using System.Collections.Generic;

namespace SoftwareDesign
{
    class Lecturer
    {
        public string Name;
        public List<Subject> Subjects = new List<Subject>();
        //public List<string> presence = new List<string>();
        public string[] Availability = new string[50];
    }
}