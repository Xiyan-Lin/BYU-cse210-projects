using System;
using System.Collections.Generic;
using System.Threading;

// ========================================
// Base class: Activity
// ========================================
public class Activity
{
    private string _name;
    private string _description;
    protected int _duration;

    public Activity(string name, string description)
    {
        _name = name;
        _description = description;
    }

    public void Start()
    {
        Console.Clear();
        Console.WriteLine($"Welcome to the {_name} Activity!");
        Console.WriteLine(_description);
        Console.Write("\nEnter duration (seconds): ");
        _duration = int.Parse(Console.ReadLine());

        Console.WriteLine("\nPrepare to begin...");
        ShowSpinner(3);
    }

    public void End()
    {
        Console.WriteLine("\nGreat job!");
        ShowSpinner(2);
        Console.WriteLine($"\nYou completed the {_name} activity for {_duration} seconds.");
        ShowSpinner(3);
    }

    // Spinner animation
    protected void ShowSpinner(int seconds)
    {
        string[] frames = { "|", "/", "-", "\\" };
        DateTime end = DateTime.Now.AddSeconds(seconds);

        int i = 0;
        while (DateTime.Now < end)
        {
            Console.Write(frames[i % 4]);
            Thread.Sleep(200);
            Console.Write("\b \b");
            i++;
        }
    }

    // Countdown animation
    protected void ShowCountdown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write(i);
            Thread.Sleep(1000);
            Console.Write("\b \b");
        }
    }
}

// ========================================
// Breathing Activity
// ========================================
public class BreathingActivity : Activity
{
    public BreathingActivity()
        : base("Breathing", 
              "This activity will help you relax by guiding your breathing. Clear your mind and focus on your breath.")
    { }

    public void Run()
    {
        Start();
        DateTime end = DateTime.Now.AddSeconds(_duration);

        while (DateTime.Now < end)
        {
            Console.Write("\nBreathe in...");
            ShowCountdown(3);

            Console.Write("\nBreathe out...");
            ShowCountdown(3);

            Console.WriteLine();
        }

        End();
    }
}

// ========================================
// Reflection Activity
// ========================================
public class ReflectionActivity : Activity
{
    private List<string> _prompts = new List<string>()
    {
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something truly selfless."
    };

    private List<string> _questions = new List<string>()
    {
        "Why was this experience meaningful to you?",
        "How did you feel when it was complete?",
        "What could you learn from this experience?",
        "How can you apply this experience in the future?",
        "What did you learn about yourself?",
        "What made this experience special?"
    };

    public ReflectionActivity()
        : base("Reflection",
              "This activity helps you reflect on moments of strength and resilience.")
    { }

    public void Run()
    {
        Start();

        Random rand = new Random();
        Console.WriteLine("\nConsider the following prompt:");
        Console.WriteLine($"--- {_prompts[rand.Next(_prompts.Count)]} ---");
        Console.WriteLine("When you have something in mind, press Enter...");
        Console.ReadLine();

        Console.WriteLine("Now reflect on the following questions:");

        DateTime end = DateTime.Now.AddSeconds(_duration);

        while (DateTime.Now < end)
        {
            string q = _questions[rand.Next(_questions.Count)];
            Console.WriteLine($"> {q}");
            ShowSpinner(4);
        }

        End();
    }
}

// ========================================
// Listing Activity
// ========================================
public class ListingActivity : Activity
{
    private List<string> _prompts = new List<string>()
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who have you helped recently?",
        "What are things you are grateful for?",
        "Who are some of your personal heroes?"
    };

    public ListingActivity()
        : base("Listing",
              "This activity helps you list positive things in your life.")
    { }

    public void Run()
    {
        Start();

        Random rand = new Random();
        string prompt = _prompts[rand.Next(_prompts.Count)];

        Console.WriteLine($"\nList as many responses as you can to the following prompt:");
        Console.WriteLine($"--- {prompt} ---");
        Console.WriteLine("\nYou may begin in:");
        ShowCountdown(3);

        List<string> items = new List<string>();

        DateTime end = DateTime.Now.AddSeconds(_duration);

        while (DateTime.Now < end)
        {
            Console.Write("> ");
            items.Add(Console.ReadLine());
        }

        Console.WriteLine($"\nYou listed {items.Count} items!");

        End();
    }
}

// ========================================
// Program (Menu)
// ========================================
class Program
{
    static void Main()
    {
        int choice = 0;

        while (choice != 4)
        {
            Console.Clear();
            Console.WriteLine("Mindfulness Program");
            Console.WriteLine("-------------------");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflection Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("4. Quit");
            Console.Write("\nSelect an option: ");

            choice = int.Parse(Console.ReadLine());

            if (choice == 1)
                new BreathingActivity().Run();
            else if (choice == 2)
                new ReflectionActivity().Run();
            else if (choice == 3)
                new ListingActivity().Run();
            else if (choice == 4)
                Console.WriteLine("Goodbye!");
        }
    }
}
