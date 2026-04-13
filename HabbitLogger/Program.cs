using Microsoft.Data.Sqlite;
using System.Globalization;

string connectionString = @"Data Source=habit-logger.db";

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
                AddHabit();
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
    using var connection = new SqliteConnection(connectionString);
    {
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS habits (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
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

void AddHabit()
{
    string userHabit;
    string date;
    int quantity;
    string unit;

    userHabit = getUserInput("Please enter the name of the habit.", "string");
    date = getUserInput("Please enter the date of the habit. (Format dd-mm-yyyy) (Enter 't' to input the today's date)", "date");
    quantity = Convert.ToInt32(getUserInput("Please enter the quantity. (No decimals allowed)", "int"));
    unit = getUserInput("Please enter the unit.", "string");

    using var connection = new SqliteConnection(connectionString);
    {
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = $"INSERT INTO habits (HABIT, DATE, QUANTITY, UNIT ) VALUES ('{userHabit}', '{date}', {quantity}, '{unit}')";

        command.ExecuteNonQuery();

        connection.Close();
    }
}

string getUserInput(string message, string typeOfData)
{
    string? userInput;
    string userInputText;
    int userInputNumber;
    DateTime temp;

    while (true)
    {
        Console.Clear();
        Console.WriteLine(message);
        userInput = Console.ReadLine();

        if (userInput == null)
            continue;

        userInputText = userInput.Trim().ToLower();

        if (typeOfData == "date")
        {
            if (userInput.Trim().ToLower() == "t")
            {
                return DateTime.Now.ToString("dd-MMM-yyyy");
            }

            if (DateTime.TryParseExact(userInput, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out temp))
            {
                return temp.ToString("dd-MMM-yyyy");
            }
        }

        if (typeOfData == "int" && int.TryParse(userInput, out userInputNumber) && userInputNumber >= 0)
            return userInputText;

        if (typeOfData == "string" && userInput != null && userInput.Length > 0)
            return userInputText;

        Console.WriteLine("Please try again.");
        Console.ReadLine();
    }
}