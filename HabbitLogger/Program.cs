using Microsoft.Data.Sqlite;

createDB();

void createDB()
{
    using var connection = new SqliteConnection(@"Data Source=habit-logger.db");
    {
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS habits (
            ID INTEGER PRIMARY KEY,
            HABIT TEXT,
            DATE TEXT,
            QUANTITY INTEGER
        )
        ";

        command.ExecuteNonQuery();

        connection.Close();
    }
}