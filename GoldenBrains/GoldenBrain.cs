namespace GoldenBrains;

public class GoldenBrain
{
    private static Dictionary<string, Dictionary<Category, int[]>> Players { get; set; } = new();

    private static Dictionary<int, Question> Questions { get; set; } = new();

    private static string? CurrentUser { get; set; }

    public static void Game()
    {
        CurrentUser = null;
        var isPlaying = true;

        while (isPlaying)
        {
            bool confirm;

            if (CurrentUser is null)
                LogIn();

            DisplayMainMenu();
            var selection = Console.ReadKey(true);

            switch (selection.Key)
            {
                case ConsoleKey.D1:
                    // Let's play!
                    PlayGame();
                    break;

                case ConsoleKey.D2:
                    // Display Leaderboard menu
                    DisplayLeaderboardMenu();
                    break;

                case ConsoleKey.D3:
                    // Display rules
                    DisplayRules();
                    Console.Clear();
                    break;

                case ConsoleKey.D9:
                    // Logout

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
        // Confirmation logic
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

    private static void DisplayLeaderboardMenu()
    {
        // Display leaderboard menu
        var isMenu = true;
        while (isMenu)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"Hello {CurrentUser}!");
            Console.WriteLine();
            Console.WriteLine("Choose your desired destination!");
            Console.WriteLine();
            Console.WriteLine("1. Player list.");
            Console.WriteLine("2. Top players.");
            Console.WriteLine("3. Player scores.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press q to return.");

            var selection = Console.ReadKey(true);
            var isValid =
                selection.Key is ConsoleKey.D1 or ConsoleKey.D2 or ConsoleKey.D3 or ConsoleKey.Q;

            while (!isValid)
            {
                selection = Console.ReadKey(true);
                isValid =
                    selection.Key
                        is ConsoleKey.D1
                            or ConsoleKey.D2
                            or ConsoleKey.D3
                            or ConsoleKey.Q;
            }

            switch (selection.Key)
            {
                case ConsoleKey.D1:
                    DisplayPlayers();
                    break;
                case ConsoleKey.D2:
                    DisplayRankScores(GetCount());
                    break;
                case ConsoleKey.D3:
                    DisplayFullScores();
                    break;
                case ConsoleKey.Q:
                    isMenu = false;
                    break;
            }
        }
    }

    private static int GetCount()
    {
        // Enter, how many Top players to see (Top3, Top5, Top1...)
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine($"Hello {CurrentUser}!");
        Console.WriteLine();
        Console.WriteLine("How many top players do you want to see?");
        Console.WriteLine();
        var input = int.TryParse(Console.ReadLine(), out int result);

        while (!input)
        {
            Console.WriteLine("Please enter a number!");
            input = int.TryParse(Console.ReadLine(), out result);
        }

        return result;
    }

    private static void DisplayPlayers()
    {
        // Display player list
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine($"Hello {CurrentUser}!");
        Console.WriteLine();
        Console.WriteLine("Player list: ");
        foreach (var player in Players)
        {
            Console.WriteLine(player.Key);
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Press q to return.");

        GoBack();
    }

    private static void DisplayRankScores(int count)
    {
        // Display player playcement based on highest score
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine($"Hello {CurrentUser}!");
        Console.WriteLine();
        Console.WriteLine($"Leaderboard Top{count}: ");

        foreach (Category category in Enum.GetValues(typeof(Category)))
        {
            var sortedUsers = Players
                .OrderByDescending(user =>
                    user.Value.ContainsKey(category) ? user.Value[category][1] : 0
                )
                .ToList();

            if (count > sortedUsers.Count)
            {
                count = sortedUsers.Count;
                Console.WriteLine(
                    $"There are only {sortedUsers.Count()} players that have played."
                );
            }
            Console.WriteLine($"{category} =>");

            var rank = 1;
            var showRank = 1;
            var lastRank = -1;

            for (var index = 0; index < count; index++)
            {
                var i = sortedUsers[index];
                if (i.Value[category][1] != 0)
                {
                    if (i.Value[category][1] != lastRank)
                        rank = showRank;

                    var placement = rank switch
                    {
                        1 => "*",
                        2 => "**",
                        3 => "***",
                        _ => "" // No symbol for playecement beyond 3rd
                    };
                    Console.WriteLine($"{i.Key}   {placement}");

                    lastRank = i.Value[category][1];
                    showRank++;
                }
            }

            Console.WriteLine();
        }

        Console.WriteLine();

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Press q to return.");

        GoBack();
    }

    private static void DisplayFullScores()
    {
        // Display all players and their scores in each category
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine($"Hello {CurrentUser}!");
        Console.WriteLine();
        Console.WriteLine("Leaderboard with scores: ");

        foreach (var user in Players)
        {
            Console.WriteLine($"{user.Key} =>");

            var sortedCategories = user
                .Value.OrderByDescending(category => category.Value[1])
                .ToList();

            foreach (var i in sortedCategories)
            {
                Console.Write($"{i.Key} => ");
                if (i.Value[1] != 0)
                    Console.WriteLine($"{i.Value[1]} points");
                else
                    Console.WriteLine("Has not played in this Category yet!");
            }

            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Press q to return.");

        GoBack();
    }

    private static void GoBack()
    {
        // Wait for correct key press
        var k = Console.ReadKey(true);
        while (k.Key != ConsoleKey.Q)
        {
            k = Console.ReadKey(true);
        }
    }

    private static void DisplayRules()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine($"Hello {CurrentUser}, please read these rules.");
        Console.WriteLine();
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

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Press q to return.");

        GoBack();
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

        if (!Players.TryGetValue(user, out var value))
        {
            Players[user] = new Dictionary<Category, int[]>
            {
                { Category.ComputerScience, [0, 0] },
                { Category.Cars, [0, 0] },
                { Category.Animals, [0, 0] }
            };
            Console.WriteLine($"{user} score initialized");
            CurrentUser = user;
        }
        else
        {
            Console.WriteLine($"{user} logged in. Score {value}");
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

    private static bool IsAnswered(Category category, int randomQ, int question)
    {
        Console.WriteLine("Are you sure? Y/N");
        var confirm = GetConfirmation();

        if (confirm)
        {
            Players[CurrentUser][category][0] += Questions[randomQ].Answers[question - 1].Score;
            return true;
        }

        return false;
    }

    private static bool CategoryMenu(out Category category)
    {
        // Select a category
        var result = Category.ComputerScience;
        Console.Clear();
        Console.WriteLine($"Hello {CurrentUser}, and please enjoy the game!");
        Console.WriteLine();
        Console.WriteLine("Category list: ");

        for (var i = 0; i < Enum.GetValues(typeof(Category)).Length; i++)
            Console.WriteLine($"{i + 1}. {Enum.GetValues(typeof(Category)).GetValue(i)}");

        Console.WriteLine();
        Console.WriteLine("Please select your category or press Q to quit.");

        var selection = Console.ReadKey(true);
        var isValid = selection.Key is ConsoleKey.D1 or ConsoleKey.D2 or ConsoleKey.D3;

        if (selection.Key == ConsoleKey.Q)
        {
            category = 0;
            return false;
        }

        while (!isValid)
        {
            selection = Console.ReadKey(true);
            isValid = selection.Key is ConsoleKey.D1 or ConsoleKey.D2 or ConsoleKey.D3;
        }

        switch (selection.Key)
        {
            case ConsoleKey.D1:
                result = Category.ComputerScience;
                break;
            case ConsoleKey.D2:
                result = Category.Cars;
                break;
            case ConsoleKey.D3:
                result = Category.Animals;
                break;
        }

        category = result;
        return true;
    }

    private static void PlayGame()
    {
        var isPlaying = CategoryMenu(out var category);

        while (isPlaying)
        {
            switch (category)
            {
                case Category.ComputerScience:
                    Questions = GetComputerScienceQuestions();
                    break;
                case Category.Cars:
                    Questions = GetCarQuestions();
                    break;
                case Category.Animals:
                    Questions = GetAnimalQuestions();
                    break;
            }

            var askedQuestions = new List<int>();
            var random = new Random();

            for (var i = 0; i < Questions.Count; i++)
            {
                int randomQ;

                do
                {
                    randomQ = random.Next(1, Questions.Count + 1);
                } while (askedQuestions.Contains(randomQ));

                askedQuestions.Add(randomQ);

                var answered = false;
                while (!answered)
                {
                    Console.Clear();
                    Console.WriteLine($"Hello {CurrentUser}, and please enjoy the game!");
                    Console.WriteLine();
                    Console.WriteLine($"Current score: {Players[CurrentUser][category][0]}\n");
                    Console.WriteLine($"Q{i + 1}: {Questions[randomQ].Text}");
                    Console.WriteLine();
                    Console.WriteLine("Select your answer:");
                    foreach (var answer in Questions[randomQ].Answers)
                        Console.WriteLine($"{answer.Number}. {answer.Text}");

                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Press q to return.");

                    var selection = Console.ReadKey(true);
                    var isValid =
                        selection.Key
                            is ConsoleKey.D1
                                or ConsoleKey.D2
                                or ConsoleKey.D3
                                or ConsoleKey.D4
                                or ConsoleKey.Q;

                    while (!isValid)
                    {
                        selection = Console.ReadKey(true);
                        isValid =
                            selection.Key
                                is ConsoleKey.D1
                                    or ConsoleKey.D2
                                    or ConsoleKey.D3
                                    or ConsoleKey.D4
                                    or ConsoleKey.Q;
                    }

                    switch (selection.Key)
                    {
                        case ConsoleKey.D1:

                            answered = IsAnswered(category, randomQ, 1);
                            continue;

                        case ConsoleKey.D2:
                            answered = IsAnswered(category, randomQ, 2);
                            continue;

                        case ConsoleKey.D3:

                            answered = IsAnswered(category, randomQ, 3);
                            continue;

                        case ConsoleKey.D4:

                            answered = IsAnswered(category, randomQ, 4);
                            continue;

                        case ConsoleKey.Q:
                            Console.WriteLine("Are you sure? Y/N");
                            var confirm = GetConfirmation();

                            if (confirm)
                            {
                                isPlaying = false;
                                return;
                            }

                            break;
                    }
                }
            }

            var topPlayer = Players
                .OrderByDescending(player => player.Value.GetValueOrDefault(category, [0, 0])[1])
                .FirstOrDefault();

            Console.Clear();
            Console.WriteLine($"Hope you enjoyed it {CurrentUser}!");
            Console.WriteLine();
            Console.WriteLine(
                $"Your score was {Players[CurrentUser][category][0]} out of {Questions.Count * 2}\n"
                    + $"And your highest score in {category} was: {Players[CurrentUser][category][1]}!"
            );

            if (topPlayer.Value[category][1] != 0)
                Console.WriteLine(
                    $"{topPlayer.Key} holds a highest score of {topPlayer.Value[category][1]} points in this category {category}!"
                );
            else
                Console.WriteLine($"No player has a score in {category} yet!");

            if (Players[CurrentUser][category][0] > topPlayer.Value[category][1])
                Console.WriteLine("Congratulations! You now have the highest score!");

            Console.WriteLine("The correct answers were:");
            // Display Questions/Answers (most points)
            Console.WriteLine();
            foreach (var question in Questions)
            {
                var top = question.Value.Answers.FirstOrDefault(score => score.Score == 2);
                Console.WriteLine(
                    $"{question.Key}. {question.Value.Text}\n\t{top.Number}. {top.Text} ({top.Score} points)\n"
                );
            }

            Console.WriteLine();

            if (Players[CurrentUser][category][1] < Players[CurrentUser][category][0])
                Players[CurrentUser][category][1] = Players[CurrentUser][category][0];

            Console.WriteLine();
            Console.WriteLine("Would you like to retry? Y/N");
            if (GetConfirmation())
            {
                Players[CurrentUser][category][0] = 0;
                PlayGame();
            }
            isPlaying = false;
        }
    }

    private static Dictionary<int, Question> GetComputerScienceQuestions()
    {
        return new Dictionary<int, Question>
        {
            {
                1,
                new Question(
                    "What does CPU stand for?",
                    new List<Answer>
                    {
                        new(1, "Central Processing Unit", 2),
                        new(2, "Centralized Processing Unit", 1),
                        new(3, "Central Processor Unit", 1),
                        new(4, "Computer Processing Unit", 0)
                    }
                )
            },
            {
                2,
                new Question(
                    "Which programming language is known for its coffee cup logo?",
                    new List<Answer>
                    {
                        new(1, "JavaScript", 1),
                        new(2, "Java", 2),
                        new(3, "C#", 0),
                        new(4, "Python", 0)
                    }
                )
            },
            {
                3,
                new Question(
                    "What is the main function of an operating system?",
                    new List<Answer>
                    {
                        new(1, "Run applications", 1),
                        new(2, "Display graphics", 0),
                        new(3, "Store data", 0),
                        new(4, "Manage hardware and software resources", 2)
                    }
                )
            },
            {
                4,
                new Question(
                    "What does HTML stand for?",
                    new List<Answer>
                    {
                        new(1, "HyperText Management Language", 1),
                        new(2, "HighText Markup Language", 0),
                        new(3, "HyperText Markup Language", 2),
                        new(4, "Hyper Tool Markup Language", 0)
                    }
                )
            },
            {
                5,
                new Question(
                    "Which of the following is a database management system?",
                    new List<Answer>
                    {
                        new(1, "Java", 1),
                        new(2, "MySQL", 2),
                        new(3, "HTML", 0),
                        new(4, "Python", 0)
                    }
                )
            },
            {
                6,
                new Question(
                    "Which of the following is a key feature of cloud computing?",
                    new List<Answer>
                    {
                        new(1, "Free access", 1),
                        new(2, "Scalability", 2),
                        new(3, "High latency", 0),
                        new(4, "Limited storage", 0)
                    }
                )
            },
            {
                7,
                new Question(
                    "What is the purpose of a compiler?",
                    new List<Answer>
                    {
                        new(1, "Store code", 0),
                        new(2, "Debug code", 0),
                        new(3, "Convert source code to machine code", 2),
                        new(4, "Execute code", 1)
                    }
                )
            },
            {
                8,
                new Question(
                    "What is the first version of Windows OS?",
                    new List<Answer>
                    {
                        new(1, "Windows 1.0", 2),
                        new(2, "Windows 95", 0),
                        new(3, "Windows XP", 0),
                        new(4, "Windows 10", 0)
                    }
                )
            },
            {
                9,
                new Question(
                    "Which company developed the Java programming language?",
                    new List<Answer>
                    {
                        new(1, "Google", 1),
                        new(2, "Sun Microsystems", 2),
                        new(3, "Microsoft", 0),
                        new(4, "IBM", 0)
                    }
                )
            },
            {
                10,
                new Question(
                    "Which algorithm is used in Google Search Engine?",
                    new List<Answer>
                    {
                        new(1, "Dijkstra", 0),
                        new(2, "Merge Sort", 0),
                        new(3, "Quicksort", 1),
                        new(4, "PageRank", 2)
                    }
                )
            }
        };
    }

    private static Dictionary<int, Question> GetCarQuestions()
    {
        return new Dictionary<int, Question>
        {
            {
                1,
                new Question(
                    "Which company makes the Mustang car?",
                    new List<Answer>
                    {
                        new(1, "Chevrolet", 0),
                        new(2, "Nissan", 1),
                        new(3, "Ford", 2),
                        new(4, "Toyota", 0)
                    }
                )
            },
            {
                2,
                new Question(
                    "What is the main function of a catalytic converter in a car?",
                    new List<Answer>
                    {
                        new(1, "Make the car go faster", 0),
                        new(2, "Increase engine efficiency", 0),
                        new(3, "Reduce harmful emissions", 2),
                        new(4, "Improve fuel economy", 1)
                    }
                )
            },
            {
                3,
                new Question(
                    "Which country is known for the luxury car brand Ferrari?",
                    new List<Answer>
                    {
                        new(1, "United Kingdom", 1),
                        new(2, "Italy", 2),
                        new(3, "Germany", 0),
                        new(4, "France", 0)
                    }
                )
            },
            {
                4,
                new Question(
                    "Which car manufacturer produces the Civic model?",
                    new List<Answer>
                    {
                        new(1, "BMW", 1),
                        new(2, "Toyota", 0),
                        new(3, "Ford", 0),
                        new(4, "Honda", 2)
                    }
                )
            },
            {
                5,
                new Question(
                    "What does the acronym ABS stand for in relation to cars?",
                    new List<Answer>
                    {
                        new(1, "Anti-lock Braking System", 2),
                        new(2, "Automatic Brake Stabilizer", 0),
                        new(3, "All Brake Safety", 0),
                        new(4, "Automated Braking System", 1)
                    }
                )
            },
            {
                6,
                new Question(
                    "What year was the first Ford Model T produced?",
                    new List<Answer>
                    {
                        new(1, "1908", 2),
                        new(2, "1920", 0),
                        new(3, "1899", 0),
                        new(4, "1912", 1)
                    }
                )
            },
            {
                7,
                new Question(
                    "Which car brand's logo features a silver star?",
                    new List<Answer>
                    {
                        new(1, "Audi", 0),
                        new(2, "Porsche", 1),
                        new(3, "Mercedes-Benz", 2),
                        new(4, "BMW", 0)
                    }
                )
            },
            {
                8,
                new Question(
                    "What is a hybrid car?",
                    new List<Answer>
                    {
                        new(1, "A car that runs on hydrogen", 1),
                        new(2, "A car that uses both gasoline and electricity", 2),
                        new(3, "A car that uses only gasoline", 0),
                        new(4, "A car that uses only electricity", 0)
                    }
                )
            },
            {
                9,
                new Question(
                    "What is the purpose of a timing belt in a car?",
                    new List<Answer>
                    {
                        new(1, "To power the alternator", 1),
                        new(2, "To filter air", 0),
                        new(3, "To synchronize the engine's camshaft and crankshaft", 2),
                        new(4, "To improve fuel efficiency", 0)
                    }
                )
            },
            {
                10,
                new Question(
                    "What does the 'K' in K-car stand for?",
                    new List<Answer>
                    {
                        new(1, "Kinetics", 0),
                        new(2, "Kilo", 0),
                        new(3, "Compact", 2),
                        new(4, "Kin", 1)
                    }
                )
            }
        };
    }

    private static Dictionary<int, Question> GetAnimalQuestions()
    {
        return new Dictionary<int, Question>
        {
            {
                1,
                new Question(
                    "What is the largest mammal on Earth?",
                    new List<Answer>
                    {
                        new(1, "Whale Shark", 1),
                        new(2, "Blue Whale", 2),
                        new(3, "African Elephant", 0),
                        new(4, "Giraffe", 0)
                    }
                )
            },
            {
                2,
                new Question(
                    "What type of animal is a Komodo dragon?",
                    new List<Answer>
                    {
                        new(1, "Turtle", 1),
                        new(2, "Lizard", 2),
                        new(3, "Snake", 0),
                        new(4, "Alligator", 0)
                    }
                )
            },
            {
                3,
                new Question(
                    "What is the fastest land animal?",
                    new List<Answer>
                    {
                        new(1, "Cheetah", 2),
                        new(2, "Lion", 0),
                        new(3, "Greyhound", 1),
                        new(4, "Giraffe", 0)
                    }
                )
            },
            {
                4,
                new Question(
                    "Which animal is known for its ability to regenerate limbs?",
                    new List<Answer>
                    {
                        new(1, "Starfish", 1),
                        new(2, "Lizard", 0),
                        new(3, "Axolotl", 2),
                        new(4, "Frog", 0)
                    }
                )
            },
            {
                5,
                new Question(
                    "Which bird is known for its colorful feathers and ability to mimic sounds?",
                    new List<Answer>
                    {
                        new(1, "Parrot", 2),
                        new(2, "Sparrow", 0),
                        new(3, "Peacock", 0),
                        new(4, "Crow", 1)
                    }
                )
            },
            {
                6,
                new Question(
                    "What is the largest species of shark?",
                    new List<Answer>
                    {
                        new(1, "Great White Shark", 0),
                        new(2, "Hammerhead Shark", 0),
                        new(3, "Whale Shark", 2),
                        new(4, "Mako Shark", 1)
                    }
                )
            },
            {
                7,
                new Question(
                    "Which animal has the longest lifespan?",
                    new List<Answer>
                    {
                        new(1, "Tortoise", 1),
                        new(2, "Whale", 0),
                        new(3, "Bowhead Whale", 2),
                        new(4, "Elephant", 0)
                    }
                )
            },
            {
                8,
                new Question(
                    "What is the only mammal capable of flight?",
                    new List<Answer>
                    {
                        new(1, "Bat", 2),
                        new(2, "Flying Squirrel", 0),
                        new(3, "Bird", 0),
                        new(4, "Insect", 1)
                    }
                )
            },
            {
                9,
                new Question(
                    "Which animal is known to have the longest migration route?",
                    new List<Answer>
                    {
                        new(1, "Humpback Whale", 2),
                        new(2, "Monarch Butterfly", 0),
                        new(3, "Arctic Tern", 1),
                        new(4, "Salmon", 0)
                    }
                )
            },
            {
                10,
                new Question(
                    "Which species of bear is native to China?",
                    new List<Answer>
                    {
                        new(1, "Polar Bear", 0),
                        new(2, "Black Bear", 0),
                        new(3, "Panda Bear", 2),
                        new(4, "Brown Bear", 1)
                    }
                )
            }
        };
    }
}

internal class Question(string text, List<Answer> answers)
{
    public string Text { get; set; } = text;
    public List<Answer> Answers { get; set; } = answers;
}

internal class Answer(int number, string text, int score)
{
    public int Number { get; set; } = number;
    public string Text { get; set; } = text;
    public int Score { get; set; } = score;
}

public enum Category
{
    ComputerScience,
    Cars,
    Animals,
}
