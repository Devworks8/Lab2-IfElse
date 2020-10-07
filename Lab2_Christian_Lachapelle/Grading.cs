/*
 * Name: Christian Lachapelle
 * Student #: A00230066
 * 
 * Title: Lab2 - If-Else
 * Version: 1.0
 * 
 * Description: Give the user the option to add/remove/modify/list students.
 *              The application automatically provides the letter grade
 *              associated with the percentage grade.
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace Lab2_Christian_Lachapelle
{
    /*
     * This class contains the grading scheme
     * It is inherited by the Student class
     */
    public class Grading
    {
        // Create a dictionary with a char datatype as a key, tuple as a value
        public Dictionary<char, Tuple<double, double>> gradingScheme =
            new Dictionary<char, Tuple<double, double>>
            {
                { 'A', new Tuple<double, double>(80.0, 100.0) },
                { 'B', new Tuple<double, double>(70.0, 79.9) },
                { 'C', new Tuple<double, double>(60, 69.9) },
                { 'D', new Tuple<double, double>(50.0, 59.9) },
                { 'F', new Tuple<double, double>(0.0, 49.9) }
            };
    }

    /*
     * This class is the student object used when adding a new student.
     * contains all the pertinent student information, and performs the
     * initial grade percentage conversion to a grade letter.
     */
    public class Student : Grading
    {
        public string StudentName { get; set; }
        public double StudentGrade { get; set; }
        public char StudentLetterGrade { get; set; }

        // Class constructor method
        public Student(string name, double grade)
        {
            StudentName = name;
            StudentGrade = grade;

            // Determine the approciate grade letter to assign
            foreach(KeyValuePair<char, Tuple<double, double>> kvp in gradingScheme)
            {
                if (StudentGrade >= kvp.Value.Item1)
                {
                    StudentLetterGrade = kvp.Key;
                    break;
                }
            }
        }
    }

    /*
     * This class contains the operation methods
     */
    public class StudentManager : FileOperations
    {
        // Create a dictionary to hold the student objects
        Dictionary<string, Student> studentDict = new Dictionary<string, Student>();
        private bool _cancel { get; set; } = false; // User cancel flag

        public StudentManager() : base()
        {
            // Call FileOperations constructor
        }

        // This method adds a new student to the dictionary
        private void AddStudent()
        {
            _cancel = false; // Reset flag
            string name; // Student's name
            double grade; // Student's percentage grade

            Console.Write("\nEnter student name: ");
            name = Console.ReadLine();

            Console.Write("Enter student percentage grade: ");

            /*
             * If the input isn't a double or out of range - try again
             * If student name is left blank - cancel operation 
             */
            while (!double.TryParse(Console.ReadLine(), out grade) | grade < 0 | grade > 100)
            {
                if (!String.IsNullOrEmpty(name))
                {
                    Console.WriteLine("ERROR: Invalid entry - Please try again");
                    Console.Write("Enter student percentage grade: ");
                }
                else
                {
                    _cancel = true; // Cancel operation
                    break;
                }
            }

            if (!_cancel)
            {
                // Add new student to dictionary
                studentDict.Add(name, new Student(name, grade));
                _isModified = true;
            }
            else
            {
                Console.WriteLine("Operation cancelled");
            }


            Console.WriteLine("\nPress any key to contiunue\n");
            Console.ReadKey(true);
        }

        // This method removes a student or removes all students
        private void RemoveStudent()
        {
            _cancel = false; // Reset flag
            string name; // Student's Name

            Console.Write("\nEnter student name to remove (* = ALL): ");
            name = Console.ReadLine();

            /*
             * If the student name isn't a valid key and 
             * the input is a "*" - Try again
             * If name is left empty - Cancel operation
             */
            while (!studentDict.ContainsKey(name) && !name.Contains("*"))
            {
                if (!String.IsNullOrEmpty(name))
                {
                    Console.WriteLine("ERROR: Student not found - Please try again");
                    Console.Write("Enter student name to remove (* = ALL): ");
                }
                else
                {
                    _cancel = true; // Cancel operation
                    break;
                }
            }

            if (name.Contains("*")) // Remove all students from dictionary
            {
                studentDict.Clear();
            }
            else if (!_cancel)
            {
                studentDict.Remove(name); // Remove single student from dictionary
                _isModified = true;
            }
            else
            {
                Console.WriteLine("Operation cancelled");
            }

            Console.WriteLine("\nPress any key to contiunue\n");
            Console.ReadKey(true);
        }

        // This method lists a student information or all students information
        private void ListStudent()
        {
            _cancel = false; // Reset flag
            string name; // Student's Name

            // Formatted string template
            string studentInfo = "Name: {0}, Percentage grade: {1, 0:F2}%, Letter grade for {2, 0:F2}% is {3}";

            Console.Write("\nEnter student name to view (* = ALL): ");
            name = Console.ReadLine();

            /*
             * If the student name isn't a valid key and 
             * the input isn't "*" - Try again
             * If name is empty - Cancel operation
             */
            while (!studentDict.ContainsKey(name) && !name.Contains("*"))
            {
                if (!String.IsNullOrEmpty(name))
                {
                    Console.WriteLine("ERROR: Student not found - Please try again");
                    Console.Write("Enter student name to view (* = ALL): ");
                }
                else
                {
                    _cancel = true; // Cancel operation
                    break;
                }
            }

            if (name.Contains("*")) // Show all students
            {
                Console.WriteLine("\n**** Listing all students ****\n");

                Dictionary<string, Student>.ValueCollection student =
                    studentDict.Values;

                foreach (Student obj in student)
                {
                    Console.WriteLine(String.Format($"{studentInfo}",
                        obj.StudentName, obj.StudentGrade,
                        obj.StudentGrade, obj.StudentLetterGrade));
                }
            }
            else if (!String.IsNullOrEmpty(name)) // Show requested student
            {
                Console.WriteLine(String.Format($"{studentInfo}",
                    studentDict[name].StudentName,
                    studentDict[name].StudentGrade,
                    studentDict[name].StudentGrade,
                    studentDict[name].StudentLetterGrade));
            }
            else
            {
                Console.WriteLine("Operation cancelled");
            }

            Console.WriteLine("\nPress any key to contiunue\n");
            Console.ReadKey(true);
        }

        // This method modifies a student's grade
        private void ModifyStudent()
        {
            _cancel = false; // reset flag
            string name; // Student's name
            double grade; // Student's percentage grade

            Console.Write("\nEnter student name you wish to modify: ");
            name = Console.ReadLine();

            Console.Write("Enter student new percentage grade: ");

            /*
             * If the input isn't a double or out of range - Try again
             * If name is empty - Cancel operation
             */
            while (!double.TryParse(Console.ReadLine(), out grade) | grade < 0 | grade > 100)
            {
                if (!String.IsNullOrEmpty(name))
                {
                    Console.WriteLine("ERROR: Invalid entry - Please try again");
                    Console.Write("Enter student percentage grade: ");
                }
                else
                {
                    _cancel = true; // Cancel operation
                    break;
                }
            }

            /*
             * If the student name isn't a valid key - Try again
             * If name is empty - Cancel operation
             */
            while (!studentDict.ContainsKey(name))
            {
                if (!String.IsNullOrEmpty(name))
                {
                    Console.WriteLine("ERROR: Student not found - Please try again");
                    Console.Write("Enter student name to modify: ");
                    name = Console.ReadLine();
                }
                else
                {
                    _cancel = true; // Cancel operation
                    break;
                }
            }

            if (!_cancel)
            {
                studentDict[name].StudentGrade = grade; // Assign new grade

                // Assign new letter grade
                foreach (KeyValuePair<char, Tuple<double, double>> kvp in studentDict[name].gradingScheme)
                {
                    if (grade >= kvp.Value.Item1)
                    {
                        studentDict[name].StudentLetterGrade = kvp.Key;
                        break;
                    }
                }

                _isModified = true;
            }
            else
            {
                Console.WriteLine("Operation cancelled");
            }

            Console.WriteLine("\nPress any key to contiunue\n");
            Console.ReadKey(true);
        }

        // This method call displays the menu
        public void CallMenu()
        {
            Console.Clear();
            Console.Write($@"
What would you like to do?
Current working file: {workingFile}

            File Operations
            ----------------
            N) New File
            O) Open File
            S) Save File
            V) View Files
            D) Delete File
            C) Close File

            Record Operations
            -----------------

            A) Add Student
            R) Remove Student
            L) List Student(s)
            M) Modify Student

            Q) Quit

Selection: ");
            char ans; // User's menu selection
            // List of all valid menu selections
            var selections = new List<char>()
            {
                'N', 'O', 'S', 'V', 'D', 'C', 'A', 'R', 'L', 'M', 'Q'
            };

            /*
             * If the input isn't a valid char and 
             * the input is not in the valid selection list - Try again
             */
            while (!Char.TryParse(Console.ReadLine(), out ans) ||
                !(selections.Contains(Char.ToUpper(ans))))
            
            {
                Console.WriteLine("ERROR: Invalid entry - Please try again");
                Console.WriteLine("\nPress any key to contiunue\n");
                Console.ReadKey(true);
                CallMenu(); // Call menu
            }

            switch (char.ToUpper(ans))
            {
                case 'N': // New file
                    if (!_isModified)
                    {
                        CreateNewSet();
                    }
                    else
                    {
                        AskToSave();
                        CreateNewSet();
                    }

                    CallMenu(); // Call the menu
                    break;

                case 'O': // Open file
                    if (!_isModified)
                    {
                        ReadSet();
                    }
                    else
                    {
                        AskToSave();
                        ReadSet();
                    }

                    CallMenu(); // Call the menu
                    break;

                case 'S': // Save file
                    WriteSet();
                    CallMenu(); // Call the menu
                    break;

                case 'V': // List files
                    ListSets();
                    CallMenu();
                    break;

                case 'D': // Delete file
                    DeleteSet();
                    CallMenu(); // Call the menu
                    break;

                case 'C': //Close file
                    if (!_isModified)
                    {
                        CloseSet();
                    }
                    else
                    {
                        AskToSave();
                        CloseSet();
                    }

                    CallMenu(); // Call the menu
                    break;

                case 'A': // Add student
                    AddStudent();
                    CallMenu(); // Call the menu
                    break;

                case 'R': // Remove Student
                    RemoveStudent();
                    CallMenu(); // Call the menu
                    break;

                case 'L': // List Student(s)
                    ListStudent();
                    CallMenu(); // Call the menu
                    break;

                case 'M': // modify Student
                    ModifyStudent();
                    CallMenu(); // Call the menu
                    break;

                default: // Exit application
                    if (!_isModified)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        AskToSave();
                        Environment.Exit(0);
                    }
                    
                    break;
            }
        }
    }

    /*
     * This class contains all file I/O operations
     */
    public class FileOperations
    {
        string filePath = AppDomain.CurrentDomain.BaseDirectory + "data/"; // Files are to be found in this subdirectory
        private bool _cancel { get; set; } = false;
        public string workingFile { get; set; }
        public bool _isModified = false; // File modification flag
        private StreamReader reader; // File stream reader
        private StreamWriter writer; // File stream writer

        // Class constructor checks if the data subdirectory exists; if not, create it
        public FileOperations()
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "data"))
            {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "data");
            }
        }

        // Read file
        public void ReadSet()
        {
            
        }

        // Write File
        public void WriteSet()
        {
            
        }

        // Close the file
        public void CloseSet()
        {
            reader.Close();
            writer.Close();
            workingFile = "";
        }

        // Validate file
        public bool ValidFile(string filename)
        {
            string path = $@"{filePath}{filename}";

            if (File.Exists(path) || filename == ".csv")
            {
                return false;
            }

            return true;
        }

        // Create a new file
        public void CreateNewSet()
        {
            _cancel = false; // Reset flag

            Console.Write("\nEnter new filename: ");
            string filename = Console.ReadLine();

            /*
             * If the the file exists - try again
             * If filename is left blank - cancel operation 
             */
            while (!ValidFile(filename))
            {
                if (!String.IsNullOrEmpty(filename))
                {
                    Console.WriteLine($"ERROR: {filename}.csv already exists - Please try again");
                    Console.Write("\nEnter new filename: ");
                    filename = Console.ReadLine();
                }
                else
                {
                    _cancel = true; // Cancel operation
                    break;
                }
            }

            if (!_cancel)
            {
                // Assign path and filename to workingFile to be referenced later on
                workingFile = $@"{filePath}{filename}.csv";
                writer = new StreamWriter(workingFile); // Assign workingFile to the StreamWriter
                reader = new StreamReader(workingFile); // Assign workingFile to the StreamReader
                _isModified = true;
            }
            else
            {
                Console.WriteLine("\nOperation cancelled");
            }


            Console.WriteLine("\nPress any key to contiunue\n");
            Console.ReadKey(true);
        }

        // Delete file
        public void DeleteSet()
        {
            string fileName;
            char ans;
            var selections = new List<char>() { 'Y', 'N' };

            Console.Write("Enter the filename you wish to delete: ");
            fileName = Console.ReadLine();

            while (!ValidFile(fileName+".csv"))
            {
                if (!String.IsNullOrEmpty(fileName))
                {
                    Console.WriteLine("ERROR: Invalid filename - Please try again");
                    Console.WriteLine("\nPress any key to continue\n");
                    Console.ReadKey(true);
                    Console.Write("Enter the filename you wish to delete: ");
                    fileName = Console.ReadLine();  
                }
                else
                {
                    _cancel = true;
                    Console.WriteLine(_cancel);
                    break;
                }  
            }

            if (!_cancel)
            {
                Console.Write($"Are you sure you wish delete {fileName}? [Y/N]: ");

                while (!Char.TryParse(Console.ReadLine(), out ans) ||
                    !(selections.Contains(Char.ToUpper(ans))))
                {
                    Console.WriteLine("ERROR: Invalid entry - Please try again");
                    Console.WriteLine("\nPress any key to continue\n");
                    Console.ReadKey(true);
                    Console.Write($"Are you sure you wish delete {fileName}? [Y/N]: ");
                }

                switch (char.ToUpper(ans))
                {
                    case 'Y':
                        Console.WriteLine($@"{filePath}{fileName}");
                        File.Delete($@"{filePath}{fileName}");
                        break;

                    case 'N':
                        Console.WriteLine("Operation cancelled");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Operation cancelled");
            }

            Console.WriteLine("\nPress any key to continue\n");
            Console.ReadKey(true);
        }

        // List all available record sets in the data subdirectory
        public void ListSets()
        {
            string[] files = Directory.GetFiles(filePath, "*.csv");
            foreach (var file in files)
            {
                Console.WriteLine(Path.GetFileName(file));
            }
            Console.WriteLine("\nPress any key to contiunue\n");
            Console.ReadKey(true);
        }

        // Ask to save if there are modifications made
        public void AskToSave()
        {
            Console.Write("Modifications were made - Do you wish to save? [Y/N]: ");
            char ans;
            var selections = new List<char>() { 'Y', 'N' };

            while (!Char.TryParse(Console.ReadLine(), out ans) ||
                !(selections.Contains(Char.ToUpper(ans))))
            {
                Console.WriteLine("ERROR: Invalid entry - Please try again");
                Console.WriteLine("\nPress any key to continue\n");
                Console.ReadKey(true);
                Console.Write("Modifications were made - Do you wish to save? [Y/N]: ");
            }

            switch (char.ToUpper(ans))
            {
                case 'Y':
                    WriteSet();
                    break;

                case 'N':
                    break;
            }
        }
    }

    public static class MainClass
    {
        public static void Main(string[] args)
        {

            StudentManager manager = new StudentManager();
            manager.CallMenu();
        }
    }
}
