using System;
using System.Collections.Generic;

namespace SoftwareDesign
{
    class Lecturer
    {
        public string name;
        public List<Subject> subjects = new List<Subject>();
        public List<string> presence = new List<string>();

        public void info()
        {
            Console.WriteLine("Type: Lecturer");
        }
    }
}