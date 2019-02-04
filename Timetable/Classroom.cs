using System;

namespace Timetable
{
    class Classroom
    {
        private string name;
        private bool availability;
        private int seats;
        private string building;
        private Equipment equipment;

        public string info()
        {
            string info;
            info = this.name + " " + this.seats + " " + this.building;
            return info;
        }

    }
}