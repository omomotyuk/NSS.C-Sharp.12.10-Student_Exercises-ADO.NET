using Student_Exercises_ADO.NET.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace Student_Exercises_ADO.NET.Data
{
    class Repository<T> where T : IRecord, new()
    {
        /// <summary>
        ///  Represents a connection to the database.
        ///   This is a "tunnel" to connect the application to the database.
        ///   All communication between the application and database passes through this connection.
        /// </summary>
        public SqlConnection Connection
        {
            get
            {
                string _connectionString = "Data Source=localhost\\SQLEXPRESS;" +
                    "Initial Catalog=StudentExercises;" +
                    "Integrated Security=True";
                return new SqlConnection(_connectionString);
            }
        }

        /// <summary>
        ///  Returns a list of all selected fields in the database
        /// </summary>
        public List<T> GetAll( string table, List<string> fields )
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = GetSelectQuery(table, fields);
                    SqlDataReader reader = cmd.ExecuteReader();

                    /*
                    Dictionary<string, int> ordinalDict = new Dictionary<string, int>();
                    foreach( string field in fields )
                    {
                        ordinalDict.Add( field, reader.GetOrdinal( field ) );
                    }
                    */
                    List<T> itemList = new List<T>();

                    Dictionary<string, string> fieldValues = new Dictionary<string, string>();
                    while (reader.Read())
                    {
                        foreach( string field in fields )
                        {
                            int position = reader.GetOrdinal(field);
                            string value = "";
                            try
                            {
                                value = reader.GetString(position);
                            } catch
                            {
                                value = reader.GetInt32(position).ToString();
                            }
                            fieldValues.Add( field, value );
                        }

                        var newItem = new T();
                        newItem.Set( fieldValues );
                        itemList.Add( newItem );

                        fieldValues.Clear();
                    }

                    reader.Close();

                    return itemList;
                }
            }
        }

        public string GetSelectQuery( string table, List<string> fields )
        {
            return String.Concat("SELECT ", String.Join(", ", fields), " FROM ", table);
        }
    }
}
