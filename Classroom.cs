using System;
using System.Collections.Generic;

namespace SoftwareDesign
{
    class Classroom
    {
        public string name;
        public bool availability = true;
        public int seats;
        public string building;
        public List<Equipment> equipment = new List<Equipment>();
    }
}