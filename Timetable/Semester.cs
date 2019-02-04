using System;

namespace Timetable
{
    class Semester
    {
        private string name;
        private string course;
        private int Stage;
        private Student students;
        private Subject subjects;

        public string info()
        {
            return this.name;
        }
    }
}