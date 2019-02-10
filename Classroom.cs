using System;
using System.Collections.Generic;

namespace SoftwareDesign
{
    class Classroom
    {
        public string Name;
        //public bool availability = true;
        public int Seats;
        public string Building;
        public List<string> Equipment = new List<string>();
        public string[] Availability = new string[50];
    }
}