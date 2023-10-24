using System.Data.SQLite;
using Kalkulator_ASP.Net.Models;

namespace Kalkulator_ASP.Net.Libraries.DatabaseLibrary
{
    public static class Database
    {
        private readonly static string filePath = "log.txt";
        private readonly static string connectionString = "Data Source=historyDatabase.sqlite;Version=3;";

        public static void CreateOrOpenDatabase()
        {
            string databaseFile = "historyDatabase.sqlite";

            if (!File.Exists(databaseFile))
            {
                SQLiteConnection.CreateFile(databaseFile);
            }

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createTableSQL = @"
            CREATE TABLE IF NOT EXISTS History (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Expression TEXT,
                DateCreated DATETIME
            )";

                using (SQLiteCommand command = new SQLiteCommand(createTableSQL, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void ReadData(HomeModel calc)
        {
            List<DatabaseModel> dataList = calc.Database;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string selectQuerySQL = "SELECT * FROM History";

                    using (SQLiteCommand command = new SQLiteCommand(selectQuerySQL, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DatabaseModel data = new DatabaseModel
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    Expression = Convert.ToString(reader["Expression"]),
                                    DateCreated = Convert.ToDateTime(reader["DateCreated"])
                                };

                                dataList.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter("log.txt"))
                {
                    sw.WriteLine(ex);
                }
            }

            calc.Database = dataList;
        }

        public static void AddEntry(string expression, DateTime date)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string insertEntrySQL = "INSERT INTO History (Expression, DateCreated) VALUES (@Expression, @DateCreated)";

                    using (SQLiteCommand command = new SQLiteCommand(insertEntrySQL, connection))
                    {
                        command.Parameters.AddWithValue("@Expression", expression);
                        command.Parameters.AddWithValue("@DateCreated", date);
                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine(ex);
                }
            }
        }
        public static void DeleteEntry(int id)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string deleteEntrySQL = "DELETE FROM History WHERE ID = @ID";

                    using (SQLiteCommand command = new SQLiteCommand(deleteEntrySQL, connection))
                    {
                        command.Parameters.AddWithValue("@ID", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine(ex);
                }
            }
        }
    }
}
