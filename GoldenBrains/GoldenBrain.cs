namespace GoldenBrains;

public class GoldenBrain
{
    private static Dictionary<string, int> Players { get; set; } = new();
    private static string? CurrentUser { get; set; }

    public static void Game()
    {
        CurrentUser = null;
        var isPlaying = true;

        while (isPlaying)
        {
            var confirm = false;

            if (CurrentUser is null)
                LogIn();

            DisplayMainMenu();
            var selection = Console.ReadKey(true);

            switch (selection.Key)
            {
                case ConsoleKey.D1:
                    // Let's play!
                    Console.WriteLine("Lets play a game...");
                    break;

                case ConsoleKey.D2:
                    // Display Leaderboard menu
                    DisplayScores();
                    break;

                case ConsoleKey.D3:
                    // Display rules
                    DisplayRules();
                    Console.Clear();
                    break;

                case ConsoleKey.D9:
                    // Logout
                    Console.Clear();
                    Console.WriteLine("Are you sure? Y/N");
                    confirm = GetConfirmation();

                    if (confirm)
                    {
                        Logout();
                        Console.Clear();
                    }

                    break;

                case ConsoleKey.Q:
                    // Quit
                    Console.Clear();
                    Console.WriteLine("Are you sure? Y/N");
                    confirm = GetConfirmation();

                    if (confirm)
                    {
                        Console.Clear();
                        Console.WriteLine("Thank you for playing! \nPlease come back soon...");
                        isPlaying = false;
                    }

                    break;

                default:
                    // Default should never happen
                    Console.WriteLine("This MUST never happen!");
                    break;
            }
        }
    }

    private static bool GetConfirmation()
    {
        var result = false;
        var sel = Console.ReadKey(true);

        while (sel.Key is not (ConsoleKey.Y or ConsoleKey.N))
            sel = Console.ReadKey(true);

        result = sel.Key switch
        {
            ConsoleKey.Y => true,
            ConsoleKey.N => false,
            _ => result
        };

        return result;
    }

    private static void DisplayScores()
    {
        Players.Values.ToList().Sort();

        Console.Clear();
        Console.WriteLine("Leaderboard: ");
        foreach (var user in Players)
        {
            Console.WriteLine($"{user.Key} => {user.Value}");
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Press q to return.");
        var k = Console.ReadKey(true);
        while (k.Key != ConsoleKey.Q)
        {
            k = Console.ReadKey(true);
        }
    }

    private static void DisplayRules()
    {
        Console.Clear();
        Console.WriteLine("Welcome to GameBrains!");
        Console.WriteLine("Objective:");
        Console.WriteLine(
            "Answer as many questions correctly as possible to maximize your score.".PadLeft(10)
        );
        Console.WriteLine("Each question has four answer options:");
        Console.WriteLine("Perfect Answer: Awards full points.".PadLeft(10));
        Console.WriteLine(
            "Half-Point Answers: Award half points each. (Some questions may have one or two of these.)".PadLeft(
                10
            )
        );
        Console.WriteLine("Incorrect Answer: Awards no points.".PadLeft(10));
        Console.WriteLine("Scoring:");
        Console.WriteLine("Correct Answer: Awards full points.".PadLeft(10));
        Console.WriteLine("Half-Point Answer: Awards half points.".PadLeft(10));
        Console.WriteLine("Incorrect Answer: Awards 0 points.".PadLeft(10));
        Console.WriteLine("Game Progression:");
        Console.WriteLine(
            "A question is presented, and you choose one of the four options.".PadLeft(10)
        );
        Console.WriteLine(
            "After answering, you see your score update and proceed to the next question.".PadLeft(
                10
            )
        );
        Console.WriteLine("Winning the Game:");
        Console.WriteLine(
            "At the end of all questions, your total score is calculated.".PadLeft(10)
        );
        Console.WriteLine(
            "Scores can be categorized to provide feedback (e.g., Beginner, Intermediate, Expert) based on your performance.".PadLeft(
                10
            )
        );

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Press q to return.");

        var k = Console.ReadKey(true);
        while (k.Key != ConsoleKey.Q)
        {
            k = Console.ReadKey(true);
        }
    }

    private static void DisplayMainMenu()
    {
        Console.Clear();

        Console.WriteLine($"Welcome, {CurrentUser}. Please choose your destination!");

        Console.WriteLine("1. Let's play!");
        Console.WriteLine("2. Leaderboards");
        Console.WriteLine("3. Game rules");
        Console.WriteLine();
        Console.WriteLine("9. Logout");
        Console.WriteLine("q. Leave :(");
        Console.WriteLine();
    }

    private static void LogIn()
    {
        if (CurrentUser is not null)
        {
            Console.WriteLine($"Welcome, {CurrentUser}");
            return;
        }

        Console.WriteLine("Hello and welcome to the game!");
        Console.WriteLine("Please enter your First and Last name to login:");
        var user = Console.ReadLine();

        while (string.IsNullOrWhiteSpace(user) || !user.Contains(" "))
        {
            Console.Clear();
            Console.WriteLine(
                "Invalid input. Please enter both your first and last name separated by a space."
            );
            Console.WriteLine("Enter your Name and Lastname:");
            user = Console.ReadLine();
        }

        if (!Players.ContainsKey(user))
        {
            Players[user] = 0;
            Console.WriteLine($"{user} score initialized");
            CurrentUser = user;
        }
        else
        {
            Console.WriteLine($"{user} logged in. Score {Players[user]}");
            CurrentUser = user;
        }
    }

    private static void Logout()
    {
        if (CurrentUser is null)
            Console.WriteLine("Please login first!");
        else
        {
            Console.WriteLine($"{CurrentUser} has just logged out!");
            CurrentUser = null;
        }
    }
}
