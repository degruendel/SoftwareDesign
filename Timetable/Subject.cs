using System;

namespace Timetable
{
    class Subject
    {
        private string name;
        private string description;
        private Equipment requirements;

        public string info()
        {
            String info;
            info = this.name + " " + this.description;
            return info;
        }
    }
}