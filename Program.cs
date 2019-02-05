using System;
using System.Collections.Generic;
using System.Xml;

namespace SoftwareDesign
{
    class Program
    {
        public static List<Student> Students = new List<Student>();
        public static List<Lecturer> Lecturers = new List<Lecturer>();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            readData();
            int c = Students.Count;
            int d = Lecturers.Count;
            foreach (Student i in Students)
            {
                Console.WriteLine(i.name + " " + i.semester);
            }
            Console.WriteLine("Es sind " + c + " Studenten registriert.");
            foreach (Lecturer i in Lecturers)
            {
                Console.WriteLine(i.name + " " + i.subjects[0] + i.subjects[1] + i.subjects[2] + i.subjects[3]);
            }
            Console.WriteLine("Es sind " + d + " Dozenten registriert.");
        }

        private static void readData()
        {
            XmlTextReader reader = new XmlTextReader("data.xml");
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "student":
                            Student student = new Student();
                            Students.Add(student);
                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    if (reader.Name == "name")
                                        student.name = reader.Value;
                                    if (reader.Name == "semester")
                                        student.semester = reader.Value;
                                }
                            }
                            break;
                        case "lecturer":
                            Lecturer lecturer = new Lecturer();
                            Lecturers.Add(lecturer);

                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    if (reader.Name == "name")
                                        lecturer.name = reader.Value;
                                    if (reader.Name.Contains("subject"))
                                    {
                                        lecturer.subjects[Int32.Parse(reader.Name.Remove(0,7))] = reader.Value;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            reader.Close();
        }
    }
}
