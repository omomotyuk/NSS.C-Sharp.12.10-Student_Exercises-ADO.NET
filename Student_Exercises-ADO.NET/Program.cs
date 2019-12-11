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
            // Insert a new exercise into the database.
            // Find all instructors in the database. Include each instructor's cohort.
            // Insert a new instructor into the database. Assign the instructor to an existing cohort.
            // Assign an existing exercise to an existing student.

            //Action_AllStudent();
        }

        static void Action_AllExercises()
        {
            var exerciseRepository = new Repository<Exercise>();
            List<string> exerciseFields = new List<string>() { "Name", "Language" };
            var allExercises = exerciseRepository.GetAll("Exercise", exerciseFields);
            Console.WriteLine("\nAll Exercises:\n" +
                                "--------------");
            foreach (var exercise in allExercises)
            {
                Console.WriteLine($"\"{exercise.Name}\" ({exercise.Language})");
            }
            Console.Write("\n");
        }

        static void Action_AllStudent()
        {
            var repository = new Repository<Student>();
            List<string> fields = new List<string>() { "FirstName", "LastName", "CohortId" };
            var allStudents = repository.GetAll("Student", fields);
            Console.WriteLine("\nAll Students:\n" +
                                "-------------");
            foreach (var student in allStudents)
            {
                Console.WriteLine($"{student.FirstName} {student.LastName} is from cohort {student.CohortId}");
            }
            Console.Write("\n");
        }
    }
}
