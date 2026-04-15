using Microsoft.Data.Sqlite;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

string connectionString = @"Data Source=habit-logger.db";

ExecuteNonQuerySQL(@"
            CREATE TABLE IF NOT EXISTS habits(
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            HABIT TEXT,
            DATE TEXT,
            QUANTITY INTEGER,
            UNIT TEXT
        )
        ");

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
                ViewHabit();
                Console.WriteLine("(Press Enter to continue)");
                Console.ReadLine();
                break;
            case "2":
                AddHabit();
                break;
            case "3":
                DeleteHabit();
                break;
            case "4":
                UpdateHabit();
                break;
            default:
                Console.WriteLine("You've entered an incorrect option. Please try again. (Press Enter to continue)");
                Console.ReadLine();
                break;
        }
    }
}

void ViewHabit()
{
    using var connection = new SqliteConnection(connectionString);
    {
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = @"SELECT * FROM habits";

        using var reader = command.ExecuteReader();

        Console.Clear();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string habit = reader.GetString(1);
            string date = reader.GetString(2);
            int quantity = reader.GetInt32(3);
            string unit = reader.GetString(4);

            Console.WriteLine($"ID: {id}\t| Habit: {habit}\t| Date: {date}\t| Quantity: {quantity}\t| Unit: {unit}");
        }

        connection.Close();
    }
}

void AddHabit()
{
    string userHabit;
    string date;
    int quantity;
    string unit;

    userHabit = GetUserHabit("Please enter the name of the habit.", "string");
    date = GetUserHabit("Please enter the date of the habit. (Format dd-mm-yyyy) (Enter 't' to input the today's date)", "date");
    quantity = Convert.ToInt32(GetUserHabit("Please enter the quantity. (No decimals allowed)", "int"));
    unit = GetUserHabit("Please enter the unit.", "string");

    ExecuteNonQuerySQL($"INSERT INTO habits (HABIT, DATE, QUANTITY, UNIT ) VALUES ('{userHabit}', '{date}', {quantity}, '{unit}')");

    Console.WriteLine($"{userHabit} has been added to the Habbit Logger! (Press Enter to continue)");
    Console.ReadLine();
}

void DeleteHabit()
{
    string? idToDelete;
    int numberOfRowsDeleted;

    while (true)
    {
        ViewHabit();

        Console.WriteLine("\n----------------------------------------------------------------------------------------------\n");
        Console.WriteLine("Please enter the ID of the habit you want to delete.");
        idToDelete = Console.ReadLine();

        if (idToDelete == null || !int.TryParse(idToDelete, out _))
        {
            Console.WriteLine("Please enter a valid ID. (Press Enter to continue)");
            Console.ReadLine();
            continue;
        }

        using var connection = new SqliteConnection(connectionString);
        {
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = $"DELETE FROM habits WHERE ID = {idToDelete}";

            numberOfRowsDeleted = command.ExecuteNonQuery();

            connection.Close();
        }

        if (numberOfRowsDeleted == 0)
        {
            Console.WriteLine("The ID you entered doesn't exist. (Press Enter to continue)");
            Console.ReadLine();
            continue;
        }

        Console.WriteLine($"The Habit with id {idToDelete} has been successfully deleted! (Press Enter to continue)");
        Console.ReadLine();
        break;
    }
}

void UpdateHabit()
{
    string? idToUpdate;
    string newUserHabit;
    string newDate;
    int newQuantity;
    string newUnit;

    int numberOfRowsUpdated;

    while (true)
    {
        ViewHabit();

        Console.WriteLine("Please enter the ID of the habit you want to modify.");
        idToUpdate = Console.ReadLine();

        if (idToUpdate == null || !int.TryParse(idToUpdate, out _))
        {
            Console.WriteLine("Please enter a correct ID. (Press Enter to continue)");
            Console.ReadLine();
            continue;
        }

        newUserHabit = GetUserHabit("Please enter the new name of the habit.", "string");
        newDate = GetUserHabit("Please enter the new date of the habit. (Format dd-mm-yyyy) (Enter 't' to input the today's date)", "date");
        newQuantity = Convert.ToInt32(GetUserHabit("Please enter the quantity. (No decimals allowed)", "int"));
        newUnit = GetUserHabit("Please enter the unit.", "string");

        using var connection = new SqliteConnection(connectionString);
        {
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = $"UPDATE habits SET HABIT = '{newUserHabit}', DATE = '{newDate}', QUANTITY = {newQuantity}, UNIT = '{newUnit}' WHERE ID = '{idToUpdate}'";

            numberOfRowsUpdated = command.ExecuteNonQuery();

            connection.Close();
        };

        if (numberOfRowsUpdated == 0)
        {
            Console.WriteLine("The ID you entered doesn't exist. (Press Enter to continue)");
            Console.ReadLine();
            continue;
        }

        Console.WriteLine($"The habit with ID {idToUpdate} has been updated successfully!");
        Console.ReadLine();
        break;
    }
}

string GetUserHabit(string message, string typeOfData)
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

        Console.WriteLine("Please enter the correct information. (Press Enter to continue)");
        Console.ReadLine();
    }
}

void ExecuteNonQuerySQL(string sqlCommand)
{
    using var connection = new SqliteConnection(connectionString);
    {
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = sqlCommand;

        command.ExecuteNonQuery();

        connection.Close();
    }
}