using Student_Exercises_ADO.NET.Data;
using Student_Exercises_ADO.NET.Model;
using System;
using System.Collections.Generic;

namespace Student_Exercises_ADO.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            // Query the database for all the Exercises.
            Action_AllExercises();

            // Find all the exercises in the database where the language is JavaScript.
            Action_IsJavaScript();

            // Insert a new exercise into the database.
            Action_Insert();
            Action_AllExercises();

            // Find all instructors in the database. Include each instructor's cohort.
            // Insert a new instructor into the database. Assign the instructor to an existing cohort.
            // Assign an existing exercise to an existing student.

            //Action_AllStudent();
        }

        static void Action_AllExercises()
        {
            Console.WriteLine("\nQuery the database for all the Exercises:");

            var repository = new Repository<Exercise>();
            List<string> fields = new List<string>() { "Name", "Language" };
            string query = GetSelectQuery("Exercise", fields);
            var exercises = repository.GetAll("Exercise", fields, query);
            Console.WriteLine("\nAll Exercises:\n" +
                                "--------------");
            foreach (var exercise in exercises)
            {
                Console.WriteLine($"\"{exercise.Name}\" ({exercise.Language})");
            }
            Console.Write("\n");
        }

        /*static void Action_AllStudent()
        {
            var repository = new Repository<Student>();
            List<string> fields = new List<string>() { "FirstName", "LastName", "CohortId" };
            var students = repository.GetAll("Student", fields);
            Console.WriteLine("\nAll Students:\n" +
                                "-------------");
            foreach (var student in students)
            {
                Console.WriteLine($"{student.FirstName} {student.LastName} is from cohort {student.CohortId}");
            }
            Console.Write("\n");
        }*/

        static void Action_IsJavaScript()
        {
            Console.WriteLine("\nFind all the exercises in the database where the language is JavaScript:");

            var repository = new Repository<Exercise>();
            List<string> fields = new List<string>() { "Name", "Language" };
            string query = GetSelectWhereQuery("Exercise", fields, "Language = 'JavaScript'");
            var exercises = repository.GetAll("Exercise", fields, query);
            Console.WriteLine("\nAll Exercises:\n" +
                                "--------------");
            foreach (var exercise in exercises)
            {
                Console.WriteLine($"\"{exercise.Name}\" ({exercise.Language})");
            }
            Console.Write("\n");
        }

        static void Action_Insert()
        {
            Console.WriteLine("\nInsert a new exercise into the database:");

            var repository = new Repository<Exercise>();

            Dictionary<string, string> data = new Dictionary<string, string>();

            data.Add("Name", "Student Exercises 2");
            data.Add("Language", "C#");

            repository.Insert("Exercise", data);
        }

        static string GetSelectQuery(string table, List<string> fields)
        {
            return String.Concat("SELECT ", String.Join(", ", fields), " FROM ", table);
        }

        static string GetSelectWhereQuery(string table, List<string> fields, string condition)
        {
            return String.Concat("SELECT ", String.Join(", ", fields), " FROM ", table, " WHERE ", condition);
        }
    }
}
