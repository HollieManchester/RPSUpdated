using System;
using System.Collections.Generic;
using System.Linq;

// Interface defining the rules for the game
interface IRules
{
    bool IsWinner(string playerChoice, string computerChoice);
    string[] GetChoices();
}

// Base class for game rules
abstract class BaseRules : IRules
{
    protected string[] choices;

    public abstract bool IsWinner(string playerChoice, string computerChoice);

    public string[] GetChoices()
    {
        return choices;
    }
}

// Default implementation of game rules
class DefaultRules : BaseRules
{
    private readonly Dictionary<string, Dictionary<string, string>> outcomes;

    public DefaultRules()
    {
        choices = new string[] { "rock", "paper", "scissors", "lizard", "spock" };

        outcomes = new Dictionary<string, Dictionary<string, string>>
        {
            { "rock", new Dictionary<string, string> { { "rock", "Draw" }, { "paper", "Computer" }, { "scissors", "Player" }, { "lizard", "Player" }, { "spock", "Computer" } } },
            { "paper", new Dictionary<string, string> { { "rock", "Player" }, { "paper", "Draw" }, { "scissors", "Computer" }, { "lizard", "Computer" }, { "spock", "Player" } } },
            { "scissors", new Dictionary<string, string> { { "rock", "Computer" }, { "paper", "Player" }, { "scissors", "Draw" }, { "lizard", "Player" }, { "spock", "Computer" } } },
            { "lizard", new Dictionary<string, string> { { "rock", "Computer" }, { "paper", "Player" }, { "scissors", "Computer" }, { "lizard", "Draw" }, { "spock", "Player" } } },
            { "spock", new Dictionary<string, string> { { "rock", "Player" }, { "paper", "Computer" }, { "scissors", "Player" }, { "lizard", "Computer" }, { "spock", "Draw" } } }
        };
    }

    public override bool IsWinner(string playerChoice, string computerChoice)
    {
        string result = outcomes[playerChoice][computerChoice];
        Console.WriteLine(result == "Draw" ? "It's a draw!" : $"{result} wins this round!");

        return result == "Player";
    }
}

// Custom implementation of game rules
class CustomRules : BaseRules
{
    private readonly Dictionary<string, Dictionary<string, string>> outcomes;

    public CustomRules()
    {
        choices = new string[] { "a", "b", "c", "d", "e" };

        outcomes = new Dictionary<string, Dictionary<string, string>>
        {
            { "a", new Dictionary<string, string> { /* Add custom rules for 'a' */ } },
            { "b", new Dictionary<string, string> { /* Add custom rules for 'b' */ } },
            { "c", new Dictionary<string, string> { /* Add custom rules for 'c' */ } },
            { "d", new Dictionary<string, string> { /* Add custom rules for 'd' */ } },
            { "e", new Dictionary<string, string> { /* Add custom rules for 'e' */ } }
        };
    }

    public override bool IsWinner(string playerChoice, string computerChoice)
    {
        if (outcomes.ContainsKey(playerChoice) && outcomes[playerChoice].ContainsKey(computerChoice))
        {
            string result = outcomes[playerChoice][computerChoice];
            Console.WriteLine(result == "Draw" ? "It's a draw!" : $"{result} wins this round!");
            return result == "Player";
        }
        else
        {
            Console.WriteLine("Invalid choice. Please choose from the available options.");
            return false;
        }
    }
}

// Class representing the game
class Game
{
    private readonly IRules rules;
    private readonly Random random;
    private int playerScore;
    private int computerScore;
    private readonly int roundsToWin;

    public Game(IRules rules, int roundsToWin = 3)
    {
        this.rules = rules;
        this.roundsToWin = roundsToWin;
        random = new Random();
    }

    public void Play()
    {
        string playerName;

        Console.WriteLine("================================");
        Console.WriteLine("Welcome to Rock, Paper, Scissors, Lizard, Spock!");
        Console.WriteLine("================================");

        Console.WriteLine("Enter your name:");
        playerName = Console.ReadLine();

        while (playerScore < roundsToWin && computerScore < roundsToWin)
        {
            string computerChoice = rules.GetChoices()[random.Next(rules.GetChoices().Length)];

            Console.WriteLine($"\n{playerName}, choose your weapon: ({string.Join(", ", rules.GetChoices())})");
            string playerChoice = Console.ReadLine().ToLower();

            if (!rules.GetChoices().Contains(playerChoice))
            {
                Console.WriteLine("Invalid choice. Please choose from the available options.");
                continue;
            }

            Console.WriteLine($"Computer chose: {computerChoice}");

            bool playerWinsRound = rules.IsWinner(playerChoice, computerChoice);
            Console.WriteLine(playerWinsRound ? $"{playerName} wins this round!" : "Computer wins this round!");

            if (playerWinsRound) playerScore++;
            else computerScore++;

            Console.WriteLine($"{playerName}: {playerScore} - Computer: {computerScore}");
        }

        Console.WriteLine(playerScore > computerScore
            ? $"\nCongratulations, {playerName}! You win the game!"
            : "\nComputer wins the game. Better luck next time!");

        Console.WriteLine("\nThanks for playing!");
    }
}

// Factory class to create instances of the game with different rules
class GameFactory
{
    public static Game CreateGame(IRules rules, int roundsToWin = 3)
    {
        return new Game(rules, roundsToWin);
    }
}

class Program
{
    static void Main()
    {
        // Instantiate the game with default rules
        IRules defaultRules = new DefaultRules();
        Game gameWithDefaultRules = GameFactory.CreateGame(defaultRules);
        gameWithDefaultRules.Play();

        // If you want to use custom rules:
        IRules customRules = new CustomRules();
        Game gameWithCustomRules = GameFactory.CreateGame(customRules);
        gameWithCustomRules.Play();
    }
}
