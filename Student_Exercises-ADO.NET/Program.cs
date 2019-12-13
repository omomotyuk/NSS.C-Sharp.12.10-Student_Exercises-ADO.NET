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
            //Action_AllStudents()

            // Find all the exercises in the database where the language is JavaScript.
            //Action_IsJavaScript();

            // Insert a new exercise into the database.
            //Action_InsertExercise();
            //Action_AllExercises();

            // Find all instructors in the database. Include each instructor's cohort.
            Action_AllInstructors();
            //Action_AllInstructorsWith(); - error!

            // Insert a new instructor into the database. Assign the instructor to an existing cohort.
            //Action_InsertInstructor();

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

        static void Action_AllStudents()
        {
            Console.WriteLine("\nQuery the database for all the Students:");

            var repository = new Repository<Student>();
            List<string> fields = new List<string>() { "FirstName", "LastName", "CohortId" };
            string query = GetSelectQuery("Student", fields);
            var students = repository.GetAll("Student", fields, query);
            Console.WriteLine("\nAll Students:\n" +
                                "-------------");
            foreach (var student in students)
            {
                Console.WriteLine($"{student.FirstName} {student.LastName} is from cohort {student.CohortId}");
            }
            Console.Write("\n");
        }

        static void Action_AllInstructors()
        {
            Console.WriteLine("\nQuery the database for all the Instructors:");

            var repository = new Repository<Instructor>();
            List<string> fields = new List<string>() { "FirstName", "LastName", "SlackHandle", "CohortId", "Speciality" };
            string query = GetSelectQuery("Instructor", fields);
            var instructors = repository.GetAll("Instructor", fields, query);
            Console.WriteLine("\nAll Instructors:\n" +
                                "-------------");
            foreach (var instructor in instructors)
            {
                Console.WriteLine($"{instructor.FirstName} {instructor.LastName} from cohort {instructor.CohortId} has following speciality - {instructor.Speciality}");
            }
            Console.Write("\n");
        }

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

        static void Action_InsertExercise()
        {
            Console.WriteLine("\nInsert a new exercise into the database:");

            var repository = new Repository<Exercise>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("Name", "Student Exercises 2");
            data.Add("Language", "C#");
            repository.Insert("Exercise", data);
        }

        static void Action_InsertInstructor()
        {
            Console.WriteLine("\nInsert a new instructor into the database:");

            var repository = new Repository<Instructor>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("FirstName", "New");
            data.Add("LastName", "Newson");
            data.Add("SlackHandle", "New Newson");
            data.Add("CohortId", "");
            data.Add("Speciality", "Sea sailing");
            repository.Insert("Instructor", data);
        }

        static void Action_AllInstructorsWith()
        {
            Console.WriteLine("\nQuery the database for all the Instructors with their Cohort name:");
            /*
             * SELECT i.FirstName, i.LastName, c.[Name] 
                FROM Instructor i
                INNER JOIN Cohort c
                ON i.CohortId = c.Id
             */
            var repository = new Repository<Record>();

            List<string> fields = new List<string>() { "FirstName", "LastName", "Cohort.[Name]" };

            string left_table = "Instructor";
            string right_table = "Cohort";

            List<string> select_fields = new List<string>();
            select_fields.Add("Instructor.FirstName");
            select_fields.Add("Instructor.LastName");
            select_fields.Add("Cohort.[Name]");

            string condition = "Instructor.CohortId = Cohort.Id";

            string query = GetInnerJoinQuery( left_table, right_table, select_fields, condition );
            Console.WriteLine($"{query}");

            List<string> parameters = new List<string>();
            parameters.Add("Cohort.Id");

            var records = repository.InnerJoin( query, fields, parameters );

            Console.WriteLine("\nAll Instructors:\n" +
                                "---------------");
            foreach (var record in records)
            {
                //Console.WriteLine($"\"{record.Name}\" ({record.Language})");
                Console.WriteLine($"\"{record}\"");
            }
            Console.Write("\n");
        }


        //
        // QueryConstructors
        //

        static string GetSelectQuery(string table, List<string> fields)
        {
            return String.Concat("SELECT ", String.Join(", ", fields), " FROM ", table);
        }

        static string GetSelectWhereQuery(string table, List<string> fields, string condition)
        {
            return String.Concat("SELECT ", String.Join(", ", fields), " FROM ", table, " WHERE ", condition);
        }

        static string GetInnerJoinQuery( string l_table, string r_table, List<string> fields, string condition )
        {
            return String.Concat( "SELECT ", String.Join(", ", fields), " FROM ", l_table, " INNER JOIN ", r_table, " ON ", condition );
        }
    }
}
