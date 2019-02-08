using System;
using System.Collections.Generic;

namespace SoftwareDesign
{
    class Semester
    {
        public string name;
        public string course;
        public int stage;
        public List<Student> students = new List<Student>();
        public List<Subject> subjects = new List<Subject>();

        public void info()
        {
            Console.WriteLine("Type: Semester, Name: " + this.name + ", Course: " + this.course + ", Stage: " + this.stage + ", Students: " + this.students + ", Subjects: " + this.subjects);
        }
    }
}