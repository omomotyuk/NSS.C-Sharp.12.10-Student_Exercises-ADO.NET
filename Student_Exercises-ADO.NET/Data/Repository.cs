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
        public List<T> GetAll( string table, List<string> fields, string query )
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //cmd.CommandText = GetSelectQuery(table, fields);
                    //Console.WriteLine($"query = {query}\n");
                    cmd.CommandText = query;
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

        public void Insert( string table, Dictionary<string,string> data)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //Console.WriteLine($"{GetInsertQuery(table, data)}");
                    cmd.CommandText = GetInsertQuery( table, data);

                    foreach( KeyValuePair<string,string> item in data )
                    {
                        string parameter = String.Concat("@", item.Key);
                        //Console.WriteLine($"{parameter}, {item.Value}");
                        cmd.Parameters.Add(new SqlParameter( parameter, item.Value ));
                    }

                    int id = (int) cmd.ExecuteScalar();
                }
            }
        }

        public string GetSelectQuery( string table, List<string> fields )
        {
            return String.Concat("SELECT ", String.Join(", ", fields), " FROM ", table);
        }

        public string GetInsertQuery(string table, Dictionary<string,string> data)
        {
            List<string> fields = new List<string>();
            List<string> values = new List<string>();
            foreach( KeyValuePair<string,string> item in data )
            {
                fields.Add(item.Key);
                values.Add(item.Value);
            }
            return String.Concat( "INSERT INTO ", table, " (", String.Join( ", ", fields), ")", " OUTPUT INSERTED.Id ", " VALUES (@", String.Join( ", @", fields), ")" );
        }
    }
}
