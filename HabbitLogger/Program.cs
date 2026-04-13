using Microsoft.Data.Sqlite;

CreateDB();
RunApp();

void RunApp ()
{
    bool running = true;

    while (running)
    {
        Console.Clear();
        Console.WriteLine("Welcome to Habit Logger!");
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine("Please select one of the following options:");
        Console.WriteLine("\t- 0 to exit the application");
        Console.WriteLine("\t- 1 to view your logged habits");
        Console.WriteLine("\t- 2 to add a new habit log");
        Console.WriteLine("\t- 3 to delete a logged habit");
        Console.WriteLine("\t- 4 to update a logged habit");

        string? userInput = Console.ReadLine();

        switch (userInput)
        {
            case "0":
                Console.WriteLine("Bye bye!");
                running = false;
                break;
            case "1":
                Console.WriteLine("UNDER CONSTRUCTION - Please be patient (Press Enter to continue)");
                Console.ReadLine();
                break;
            case "2":
                Console.WriteLine("UNDER CONSTRUCTION - Please be patient (Press Enter to continue)");
                Console.ReadLine();
                break;
            case "3":
                Console.WriteLine("UNDER CONSTRUCTION - Please be patient (Press Enter to continue)");
                Console.ReadLine();
                break;
            case "4":
                Console.WriteLine("UNDER CONSTRUCTION - Please be patient (Press Enter to continue)");
                Console.ReadLine();
                break;
            default:
                Console.WriteLine("You've entered an incorrect option. Please try again. (Press Enter to continue)");
                Console.ReadLine();
                break;
        }
    }
}

void CreateDB()
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
            QUANTITY INTEGER,
            UNIT TEXT
        )
        ";

        command.ExecuteNonQuery();

        connection.Close();
    }
}