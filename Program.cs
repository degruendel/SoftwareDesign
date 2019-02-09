using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

namespace SoftwareDesign
{
    class Program
    {
        public static List<Semester> Semesters = new List<Semester>();
        public static List<Lecturer> Lecturers = new List<Lecturer>();
        public static List<Classroom> Classrooms = new List<Classroom>();
        public static List<Subject> allSubjects = new List<Subject>();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            readData();
            createTimetable();
            //Console.WriteLine(semester.name);

            /* int c = Students.Count;
            int d = Lecturers.Count;
            int e = Classrooms.Count;
            foreach (Student i in Students)
            {
                Console.WriteLine(i.name + " " + i.semester);
            }
            Console.WriteLine("Es sind " + c + " Studenten registriert.");
            foreach (Lecturer i in Lecturers)
            {
                Console.WriteLine(i.name);
                foreach (Subject j in i.subjects)
                {
                    Console.WriteLine(j.name);
                }
                foreach (string k in i.presence)
                {
                    Console.WriteLine(k);
                }
            }
            Console.WriteLine("Es sind " + d + " Dozenten registriert.");
            foreach (Classroom i in Classrooms)
            {
                Console.WriteLine(i.name + " " + i.building);
                foreach (Equipment j in i.equipment)
                {
                    Console.WriteLine(j.name);
                }
            }
            Console.WriteLine("Es sind " + e + " Räume registriert."); */
            //Lecturer L = new Lecturer();
            //L.info("Müller");
        }

        public static void createTimetable()
        {
            foreach (Lecturer lecturer in Lecturers)
            {
                for (int i = 0; i < 50; i++)
                {
                    if (lecturer.availability[i] == "free")
                    {
                        int selectorsemester = 0;
                        Semester selectedsemester = Semesters[selectorsemester];
                        foreach (Subject subject in lecturer.subjects)
                        {
                            if (selectedsemester.subjects.Exists(e => e.name == subject.name))
                            {
                                selectedsemester.subjects.Remove(subject);
                                Console.WriteLine(lecturer.name + " unterrichtet " + subject.name + " in " + selectedsemester.name + " im Block " + i);
                            }
                        }
                    }
                }
            }
        }

        /* public static void createTimetable()
        {
            int course = 0;
            foreach (Semester sem in Semesters)
            {
                int studentamount = sem.students;
                foreach (Subject sub in sem.subjects)
                {
                    List<Classroom> matchingrooms = new List<Classroom>();
                    foreach (Classroom cla in Classrooms)
                    {
                        if (cla.availability[course] == null && cla.seats >= studentamount)
                        {
                            foreach (string req in sub.requirements)
                            {
                                if (cla.equipment.Contains(req))
                                    matchingrooms.Add(cla);
                            }
                        }
                    }
                    Classroom smallest = matchingrooms[0];
                    foreach (Classroom mat in matchingrooms)
                    {
                        if (mat.seats < smallest.seats)
                            smallest = mat;
                    }
                    Console.WriteLine("Found: " + smallest.name);
                    Classroom found = Classrooms.Find(room => room.name == smallest.name);
                    found.availability[course] == "reserved";

                    List<Lecturer> matchinglecturers = new List<Lecturer>();
                    foreach (Lecturer lec in Lecturers)
                    {
                        if (lec.subjects.Contains(sub))
                        {

                        }
                    }
                }
            }
        } */
        /* private static void createTimetable()
        {
            foreach (Semester semester in Semesters)
            {
                foreach (Subject subject in semester.subjects)
                {
                    //Sucht ale Räume, die groß genug sind
                    List<Classroom> matchsize = Classrooms.FindAll(room => room.seats >= semester.students || room.availability == true);
                    
                    foreach (Classroom room in matchsize.ToList())
                    {
                        foreach (String require in subject.requirements)
                        {
                            //Equipment nicht ausreichend
                            if (!room.equipment.Contains(require))
                            {
                                matchsize.Remove(room);
                            }
                        }
                    }
                    bool isEmpty = !matchsize.Any();
                        if (isEmpty)
                            Console.WriteLine("Keine Räume verfügbar.");
                        else
                        {
                            int countseats;
                            foreach (Classroom room in matchsize.ToList)
                            {
                                countseats = room.seats - semester.students;

                            }
                        }
                            Console.WriteLine("For " + semester.name + " " + subject.name + " Raum: " + matchsize.All.name + " " + matchsize.All.seats);
                }
            }
        } */
        private static void readData()
        {
            XmlTextReader reader = new XmlTextReader("data.xml");
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "semester":
                            Semester semester = new Semester();
                            Semesters.Add(semester);
                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    if (reader.Name == "name")
                                        semester.name = reader.Value;
                                    if (reader.Name == "students")
                                        semester.students = Int32.Parse(reader.Value);
                                    if (reader.Name.Contains("subject"))
                                    {
                                        Subject semestersubject = allSubjects.Find(s => s.name == reader.Value);
                                        semester.subjects.Add(semestersubject);
                                    }
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
                                        Subject lecturersubject = allSubjects.Find(s => s.name == reader.Value);
                                        lecturer.subjects.Add(lecturersubject);
                                    }
                                    if (reader.Name == "presence")
                                    {
                                        string[] words = reader.Value.Split(',');
                                        foreach (string word in words)
                                        {
                                            lecturer.availability[System.Convert.ToInt32(word)] = "free";
                                        }
                                    }
                                }
                            }
                            break;
                        case "classroom":
                            Classroom classroom = new Classroom();
                            Classrooms.Add(classroom);
                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    if (reader.Name == "name")
                                        classroom.name = reader.Value;
                                    if (reader.Name == "seats")
                                        classroom.seats = Int32.Parse(reader.Value);
                                    if (reader.Name == "building")
                                        classroom.building = reader.Value;
                                    if (reader.Name.Contains("equipment"))
                                        classroom.equipment.Add(reader.Value);
                                }
                            }
                            break;
                        case "subject":
                            Subject subject = new Subject();
                            allSubjects.Add(subject);
                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    if (reader.Name == "name")
                                        subject.name = reader.Value;
                                    if (reader.Name == "description")
                                        subject.description = reader.Value;
                                    if (reader.Name.Contains("requirement"))
                                        subject.requirements.Add(reader.Value);
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
