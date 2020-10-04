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
        bool _cancel = false; // User cancel flag

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
            1) Open File
            2) Save File
            3) Delete File
            4) Close File

            Subset Operations
            -----------------
            5) New Subset
            6) Delete Subset

            Record Operations
            -----------------

            7) Add Student
            8) Remove Student
            9) List Student(s)
            0) Modify Student

            Q) Quit

Selection: ");
            char ans; // User's menu selection

            /*
             * If the input isn't a valid byte and 
             * the input is out of range - Try again
             */
            while (!char.TryParse(Console.ReadLine(), out ans) && !((byte)ans >= 48 && (byte)ans <= 57) && (byte)ans != 81 && (byte)ans != 113)
            {

                Console.WriteLine(ans);
                Console.WriteLine((byte)ans);
                Console.WriteLine(!((byte)ans >= 48 && (byte)ans <= 57));
                Console.WriteLine((byte)ans != 81);
                Console.WriteLine((byte)ans != 113);

                Console.WriteLine("ERROR: Invalid entry - Please try again");
                Console.WriteLine("\nPress any key to contiunue\n");
                Console.ReadKey(true);
                CallMenu(); // Call menu
            }

            switch ((byte)ans)
            {
                case 49: // Open file
                    ReadSet(); // Need to validate file
                    CallMenu(); // Call the menu
                    break;

                case 50: // Save file
                    WriteSet();
                    CallMenu(); // Call the menu
                    break;

                case 51: // Delete file
                    DeleteSet();
                    CallMenu(); // Call the menu
                    break;

                case 52: //Close file
                    CloseSet();
                    CallMenu(); // Call the menu
                    break;

                case 53: // New subset
                    CreateNewSubset();
                    CallMenu(); // Call the menu
                    break;

                case 54: // Delete subset
                    DeleteSubset();
                    CallMenu(); // Call the menu
                    break;

                case 55: // Add student
                    AddStudent();
                    CallMenu(); // Call the menu
                    break;

                case 56: // Remove Student
                    RemoveStudent();
                    CallMenu(); // Call the menu
                    break;

                case 57: // List Student(s)
                    ListStudent();
                    CallMenu(); // Call the menu
                    break;

                case 48: // modify Student
                    ModifyStudent();
                    CallMenu(); // Call the menu
                    break;

                default: // Exit application
                    Environment.Exit(0);
                    break;
            }
        }
    }

    /*
     * This class contains all file I/O operations
     */
    public class FileOperations
    {
        const string filePath = @"./data"; // Files are to be found in this subdirectory
        public string workingFile;

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
            
        }

        // Validate file
        public void ValidateFile()
        {
            
        }

        // Create a new file
        public void CreateNewSet()
        {
            
        }

        // Delete file
        public void DeleteSet()
        {
            
        }

        // Change working file
        public void ChangeSet()
        {
            
        }

        // Create a new set of records in file
        public void CreateNewSubset()
        {
            
        }

        // Delete subset from file
        public void DeleteSubset()
        {
            
        }

        // List all available record sets in the data subdirectory
        public void ListSets()
        {
            string[] files = Directory.GetFiles(filePath, "*.cvs");
            foreach (var file in files)
            {
                Console.WriteLine(file);
            }
        }
    }

    public class MainClass
    {
        public static void Main(string[] args)
        {

            StudentManager manager = new StudentManager();
            manager.CallMenu();
        }
    }
}
