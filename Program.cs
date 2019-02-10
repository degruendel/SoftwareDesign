using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

namespace SoftwareDesign
{
    class Program
    {
        private static List<Semester> _allSemesters = new List<Semester>();
        private static List<Lecturer> _allLecturers = new List<Lecturer>();
        private static List<Classroom> _allClassrooms = new List<Classroom>();
        private static List<Subject> _allSubjects = new List<Subject>();
        private static List<Timetable> _allTimetables = new List<Timetable>();
        static void Main(string[] args)
        {
            Console.WriteLine("Hallo Stundenplan!");
            ReadData();
            CreateTimetable();
            PrintTimetable("MIB1");
            CheckWpm("MIB1");

        }
        public static void CreateTimetable()
        {
            int lengthLecturer = _allLecturers.Count();
            int lenghtSemester = _allSemesters.Count();
            int lenghtBlocks = 50;

            for (int b = 0; b < lenghtBlocks; b++)
            {

                for (int a = 0; a < lengthLecturer; a++)
                {
                    Lecturer selectedLecturer = _allLecturers[a];
                    for (int c = 0; c < lenghtSemester; c++)
                    {
                        Semester selectedSemester = _allSemesters[c];
                        if (selectedSemester.Availability[b] != "reserved")
                        {
                            foreach (Subject selectedSubject in selectedLecturer.Subjects)
                            {
                                if (selectedLecturer.Availability[b] == "free")
                                {
                                    if (selectedSemester.Subjects.Exists(s => s.Name == selectedSubject.Name))
                                    {
                                        List<Classroom> matchingRooms = new List<Classroom>();
                                        foreach (Classroom room in _allClassrooms)
                                        {
                                            if (room.Availability[b] != "reserved" && room.Seats >= selectedSemester.Students)
                                            {
                                                foreach (string require in selectedSubject.Requirements)
                                                {
                                                    if (room.Equipment.Contains(require))
                                                    {
                                                        matchingRooms.Add(room);
                                                    }

                                                }

                                            }
                                        }
                                        bool isEmpty = !matchingRooms.Any();
                                        if (isEmpty == false)
                                        {
                                            Classroom smallestRoom = matchingRooms[0];
                                            foreach (Classroom matching in matchingRooms)
                                            {
                                                if (matching.Seats < smallestRoom.Seats)
                                                {
                                                    smallestRoom = matching;
                                                }
                                            }
                                            smallestRoom.Availability[b] = "reserved";
                                            selectedSemester.Subjects.Remove(selectedSubject);
                                            selectedSemester.Availability[b] = "reserved";
                                            selectedLecturer.Availability[b] = "reserved";
                                            SaveInTimetable(b, selectedSemester, selectedSubject, smallestRoom, selectedLecturer);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private static void CheckWpm(string semestername)
        {
            Timetable currentTimetable;
            Timetable wpmTimetable;
            currentTimetable = _allTimetables.Find(t => t.Name == semestername);
            wpmTimetable = _allTimetables.Find(w => w.Name == "WPM");
            Console.WriteLine("Du kannst belegen: \n");
            for (int i = 0; i < 50; i++)
            {
                if (currentTimetable.Table[i].Contains("FREI"))
                {
                    if (!wpmTimetable.Table[i].Contains("FREI"))
                    {
                        Console.WriteLine(wpmTimetable.Table[i] + "\n");
                    }
                }
            }
        }
        private static void SaveInTimetable(int b, Semester selectedSemester, Subject selectedSubject, Classroom smallestRoom, Lecturer selectedLecturer)
        {
            Timetable currentTimetable;
            currentTimetable = _allTimetables.Find(t => t.Name == selectedSemester.Name);
            string stringBuilder;
            stringBuilder = selectedSubject.Name + " (" + b + ")\n" + smallestRoom.Building + "." + smallestRoom.Name + "\n" + selectedLecturer.Name;
            currentTimetable.Table[b] = stringBuilder;
            currentTimetable = _allTimetables.Find(t => t.Name == selectedLecturer.Name);
            stringBuilder = selectedSubject.Name + "\n" + smallestRoom.Building + "." + smallestRoom.Name;
            currentTimetable.Table[b] = stringBuilder;
            currentTimetable = _allTimetables.Find(t => t.Name == smallestRoom.Name);
            stringBuilder = selectedSubject.Name + "\n" + selectedLecturer.Name + "\n" + selectedSemester.Name;
            currentTimetable.Table[b] = stringBuilder;
        }
        public static void PrintTimetable(string name)
        {
            Console.WriteLine("\nStundenplan für " + name + "\n");
            Timetable print = _allTimetables.Find(x => x.Name == name);
            foreach (string block in print.Table)
            {
                Console.WriteLine(block);
                Console.WriteLine();
            }
        }
        private static void ReadData()
        {
            XmlTextReader reader = new XmlTextReader("timetableData.xml");
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "semester":
                            Semester semester = new Semester();
                            _allSemesters.Add(semester);
                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    if (reader.Name == "name")
                                        semester.Name = reader.Value;
                                    Timetable timetable = new Timetable();
                                    _allTimetables.Add(timetable);
                                    timetable.Name = reader.Value;
                                    for (int i = 0; i < 50; i++)
                                        timetable.Table.Add("Block " + i + "\nFREI\n");

                                    if (reader.Name == "students")
                                        semester.Students = Int32.Parse(reader.Value);
                                    if (reader.Name.Contains("subject"))
                                    {
                                        Subject semestersubject = _allSubjects.Find(s => s.Name == reader.Value);
                                        semester.Subjects.Add(semestersubject);
                                    }
                                }
                            }
                            break;
                        case "lecturer":
                            Lecturer lecturer = new Lecturer();
                            _allLecturers.Add(lecturer);
                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    if (reader.Name == "name")
                                        lecturer.Name = reader.Value;
                                    Timetable timetable = new Timetable();
                                    _allTimetables.Add(timetable);
                                    timetable.Name = reader.Value;
                                    for (int i = 0; i < 50; i++)
                                        timetable.Table.Add("Block " + i + "\nFREI\n");
                                    if (reader.Name.Contains("subject"))
                                    {
                                        Subject lecturersubject = _allSubjects.Find(s => s.Name == reader.Value);
                                        lecturer.Subjects.Add(lecturersubject);
                                    }
                                    if (reader.Name == "presence")
                                    {
                                        string[] words = reader.Value.Split(',');
                                        foreach (string word in words)
                                        {
                                            lecturer.Availability[System.Convert.ToInt32(word)] = "free";
                                        }
                                    }
                                }
                            }
                            break;
                        case "classroom":
                            Classroom classroom = new Classroom();
                            _allClassrooms.Add(classroom);
                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    if (reader.Name == "name")
                                        classroom.Name = reader.Value;
                                    Timetable timetable = new Timetable();
                                    _allTimetables.Add(timetable);
                                    timetable.Name = reader.Value;
                                    for (int i = 0; i < 50; i++)
                                        timetable.Table.Add("Block " + i + "\nFREI\n");
                                    if (reader.Name == "seats")
                                        classroom.Seats = Int32.Parse(reader.Value);
                                    if (reader.Name == "building")
                                        classroom.Building = reader.Value;
                                    if (reader.Name.Contains("equipment"))
                                        classroom.Equipment.Add(reader.Value);
                                }
                            }
                            break;
                        case "subject":
                            Subject subject = new Subject();
                            _allSubjects.Add(subject);
                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    if (reader.Name == "name")
                                        subject.Name = reader.Value;
                                    if (reader.Name == "description")
                                        subject.Description = reader.Value;
                                    if (reader.Name.Contains("requirement"))
                                        subject.Requirements.Add(reader.Value);
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