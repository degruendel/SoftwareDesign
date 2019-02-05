using System;
using System.Collections.Generic;
using System.Xml;

namespace SoftwareDesign
{
    class Program
    {
        public static List<Student> Students = new List<Student>();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            readData();
            foreach (Student i in Students)
            {
                Console.WriteLine(i.name + " " + i.semester);
            }
        }

        private static void readData()
        {
            XmlTextReader reader = new XmlTextReader("data.xml");

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    Student student = new Student();
                    Students.Add(student);
                    switch (reader.Name)
                    {
                        case "name":
                            student.name = reader.ReadString();
                            //Console.WriteLine(student.name);
                            break;
                        case "semester":
                            student.semester = reader.ReadString();
                            break;
                    }
                }
            }
            reader.Close();
        }
    }
}
