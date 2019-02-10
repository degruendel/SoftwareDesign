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
        public static List<Timetable> allTimetables = new List<Timetable>();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            readData();
            createTimetable();
            printTimetableSemester("102");
        }

        public static void createTimetable()
        {
            int lengthlecturer = Lecturers.Count();
            int lenghtsemester = Semesters.Count();
            int lenghtblocks = 50;

            for (int b = 0; b < lenghtblocks; b++)
            {

                for (int a = 0; a < lengthlecturer; a++)
                {
                    Lecturer selectedLecturer = Lecturers[a];
                    for (int c = 0; c < lenghtsemester; c++)
                    {
                        Semester selectedSemester = Semesters[c];
                        if (selectedSemester.availability[b] != "reserved")
                        {
                            foreach (Subject selectedSubject in selectedLecturer.subjects)
                            {
                                if (selectedLecturer.availability[b] == "free")
                                {
                                    if (selectedSemester.subjects.Exists(e => e.name == selectedSubject.name))
                                    {
                                        List<Classroom> matchingRooms = new List<Classroom>();
                                        foreach (Classroom room in Classrooms)
                                        {
                                            if (room.availability[b] != "reserved" && room.seats >= selectedSemester.students)
                                            {
                                                foreach (string require in selectedSubject.requirements)
                                                {
                                                    if (room.equipment.Contains(require))
                                                    {
                                                        matchingRooms.Add(room);
                                                    }

                                                }

                                            }
                                        }
                                        bool isEmpty = !matchingRooms.Any();
                                        if (isEmpty == false)
                                        {
                                            Classroom smallest = matchingRooms[0];
                                            foreach (Classroom matching in matchingRooms)
                                            {
                                                if (matching.seats < smallest.seats)
                                                {
                                                    smallest = matching;
                                                }
                                            }
                                            smallest.availability[b] = "reserved";
                                            selectedSemester.subjects.Remove(selectedSubject);
                                            selectedSemester.availability[b] = "reserved";
                                            selectedLecturer.availability[b] = "reserved";
                                            saveInTimetable(b, selectedSemester, selectedSubject, smallest, selectedLecturer);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        static void saveInTimetable(int b, Semester selectedSemester, Subject selectedSubject, Classroom smallest, Lecturer selectedLecturer)
        {
            Timetable currentTimetable;
            currentTimetable = allTimetables.Find(t => t.name == selectedSemester.name);
            string stringBuilder;
            stringBuilder = selectedSubject.name + "\n" + smallest.building + "." + smallest.name + "\n" + selectedLecturer.name;
            currentTimetable.table[b] = stringBuilder;
            currentTimetable = allTimetables.Find(t => t.name == selectedLecturer.name);
            stringBuilder = selectedSubject.name + "\n" + smallest.building + "." + smallest.name;
            currentTimetable.table[b] = stringBuilder;
            currentTimetable = allTimetables.Find(t => t.name == smallest.name);
            stringBuilder = selectedSubject.name + "\n" + selectedLecturer.name + "\n" + selectedSemester.name;
            currentTimetable.table[b] = stringBuilder;
        }

        public static void printTimetableSemester(string name)
        {
            Console.WriteLine("\nTimetable for " + name + "\n");
            Timetable print = allTimetables.Find(x => x.name == name);
            foreach (string block in print.table)
            {
                Console.WriteLine(block);
                Console.WriteLine();
            }
        }

        /* public static void createTimetable()
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
                                    Timetable timetable = new Timetable();
                                    allTimetables.Add(timetable);
                                    timetable.name = reader.Value;
                                    for (int i = 0; i < 50; i++)
                                        timetable.table.Add("Block " + i + "\nFREI\n");

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
                                    Timetable timetable = new Timetable();
                                    allTimetables.Add(timetable);
                                    timetable.name = reader.Value;
                                    for (int i = 0; i < 50; i++)
                                        timetable.table.Add("Block " + i + "\nFREI\n");
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
                                    Timetable timetable = new Timetable();
                                    allTimetables.Add(timetable);
                                    timetable.name = reader.Value;
                                    for (int i = 0; i < 50; i++)
                                        timetable.table.Add("Block " + i + "\nFREI\n");
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
