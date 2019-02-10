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
                        bool semesterIsFree = false;

                        foreach (Subject subject in lecturer.subjects)
                        {
                            while (semesterIsFree == false)
                            {
                                Semester checkSemester = Semesters[selectorsemester];
                                if (checkSemester.availability[i] == "reserved")
                                {
                                    selectorsemester++;
                                }
                                else
                                {
                                    semesterIsFree = true;
                                }
                            }
                            Semester selectedsemester = Semesters[selectorsemester];
                            if (selectedsemester.subjects.Exists(e => e.name == subject.name))
                            {
                                List<Classroom> matchingRooms = new List<Classroom>();
                                foreach (Classroom room in Classrooms)
                                {
                                    if (room.availability[i] != "reserved" && room.seats >= selectedsemester.students)
                                    {
                                        foreach (string require in subject.requirements)
                                        {
                                            if (room.equipment.Contains(require))
                                            {
                                                matchingRooms.Add(room);
                                            }
                                        }
                                    }
                                }
                                Classroom smallest = matchingRooms[0];
                                foreach (Classroom matching in matchingRooms)
                                {
                                    if (matching.seats < smallest.seats)
                                        smallest = matching;
                                }
                                smallest.availability[i] = "reserved";
                                selectedsemester.subjects.Remove(subject);
                                selectedsemester.availability[i] = "reserved";
                                Console.WriteLine(lecturer.name + " unterrichtet " + subject.name + " in " + selectedsemester.name + " im Block " + i + " im Raum " + smallest.name);
                            }
                        }
                    }
                }
            }
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
