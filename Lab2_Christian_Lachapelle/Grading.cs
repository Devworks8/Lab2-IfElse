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

namespace Lab2_Christian_Lachapelle
{
    /*
     * This class contains the grading scheme
     * It is inherited by the Student class
     */
    class Grading
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
    class Student : Grading
    {
        public string studentName { get; set; }
        public double studentGrade { get; set; }
        public char studentLetterGrade { get; set; }

        // Class constructor method
        public Student(string name, double grade)
        {
            studentName = name;
            studentGrade = grade;

            // Determine the approciate grade letter to assign
            foreach(KeyValuePair<char, Tuple<double, double>> kvp in gradingScheme)
            {
                if (studentGrade >= kvp.Value.Item1)
                {
                    studentLetterGrade = kvp.Key;
                    break;
                }
            }
        }
    }

    /*
     * This class contains the operation methods
     */
    public class StudentManager
    {
        // Create a dictionary to hold the student objects
        Dictionary<string, Student> studentDict = new Dictionary<string, Student>();
        bool _cancel = false; // User cancel flag

        // This method adds a new student to the dictionary
        private void addStudent()
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
            callMenu(); // Call the menu
        }

        // This method removes a student or removes all students
        private void removeStudent()
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
            callMenu(); // Call menu
        }

        // This method lists a student information or all students information
        private void listStudent()
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
                        obj.studentName, obj.studentGrade,
                        obj.studentGrade, obj.studentLetterGrade));
                }
            }
            else if (!String.IsNullOrEmpty(name)) // Show requested student
            {
                Console.WriteLine(String.Format($"{studentInfo}",
                    studentDict[name].studentName,
                    studentDict[name].studentGrade,
                    studentDict[name].studentGrade,
                    studentDict[name].studentLetterGrade));
            }
            else
            {
                Console.WriteLine("Operation cancelled");
            }

            Console.WriteLine("\nPress any key to contiunue\n");
            Console.ReadKey(true);
            callMenu(); // Call menu
        }

        // This method modifies a student's grade
        private void modifyStudent()
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
                studentDict[name].studentGrade = grade; // Assign new grade

                // Assign new letter grade
                foreach (KeyValuePair<char, Tuple<double, double>> kvp in studentDict[name].gradingScheme)
                {
                    if (grade >= kvp.Value.Item1)
                    {
                        studentDict[name].studentLetterGrade = kvp.Key;
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
            callMenu(); // Call menu
        }

        // This method call displays the menu
        public void callMenu()
        {
            Console.Clear();
            Console.Write(@"
What would you like to do?

            1) Add Student
            2) Remove Student
            3) List Student(s)
            4) Modify Student
            0) Quit

Selection: ");
            byte ans; // User's menu selection
          
            /*
             * If the input isn't a valid byte and 
             * the input is out of range - Try again
             */
            while (!byte.TryParse(Console.ReadLine(), out ans) || !(ans >= 0) || !(ans <= 4))
            {
                Console.WriteLine("ERROR: Invalid entry - Please try again");
                Console.WriteLine("\nPress any key to contiunue\n");
                Console.ReadKey(true);
                callMenu(); // Call menu
            }

            switch (ans)
            {
                case 1: // Add student
                    addStudent();
                    break;

                case 2: // Remove Student
                    removeStudent();
                    break;

                case 3: // List Student(s)
                    listStudent();
                    break;

                case 4: // modify Student
                    modifyStudent();
                    break;

                default: // Exit application
                    Environment.Exit(0);
                    break;
            }
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {

            StudentManager manager = new StudentManager();
            manager.callMenu();
        }
    }
}
