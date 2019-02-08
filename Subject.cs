using System;
using System.Collections.Generic;

namespace SoftwareDesign
{
    class Subject
    {
        public string name;
        public string description;
        public List<Equipment> requirements = new List<Equipment>();

        public void info()
        {
            Console.WriteLine("Type: Subject");
        }
    }
}