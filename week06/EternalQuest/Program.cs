using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/*
  Eternal Quest Program - Program.cs
  ----------------------------------
  Features:
  - Implements Goal base class (abstract) and 3 derived classes:
      SimpleGoal, EternalGoal, ChecklistGoal
  - Polymorphism: RecordEvent() returns awarded points and updates state
  - Encapsulation: private fields and public methods for access
  - Menu: Create goal, List goals, Record event, Show score, Save, Load, Quit
  - Save/Load using simple text format: Type|Title|Desc|... (see Goal.FromFileLine)
  - Extras (exceed requirements):
      * Leveling system: user levels up every 1000 points (simple text message)
      * Badges: awarding a badge when a checklist is completed
      * Input validation helpers for nicer UX
  To run: place this file in a console project and `dotnet run`.
*/

#region Goal Classes

public abstract class Goal
{
    private string _title;
    private string _description;
    private int _points; // base points for one completion

    public Goal(string title, string description, int points)
    {
        _title = title;
        _description = description;
        _points = points;
    }

    public string GetTitle() => _title;
    public string GetDescription() => _description;
    public int GetBasePoints() => _points;

    // RecordEvent: perform the goal action; returns points awarded
    public abstract int RecordEvent();

    // Get textual status for listing
    public abstract string GetStatus();

    // Convert to file line
    public abstract string ToFileLine();

    // Factory to create Goal from file line
    public static Goal FromFileLine(string line)
    {
        // Format:
        // Simple|title|desc|points|isComplete
        // Eternal|title|desc|points
        // Checklist|title|desc|points|timesRequired|timesCompleted|bonus
        var parts = line.Split('|');
        if (parts.Length == 0) return null;
        string type = parts[0];

        if (type == "Simple")
        {
            string title = parts[1];
            string desc = parts[2];
            int points = int.Parse(parts[3]);
            bool isComplete = bool.Parse(parts[4]);
            var g = new SimpleGoal(title, desc, points);
            if (isComplete) g.MarkCompleteFromLoad();
            return g;
        }
        else if (type == "Eternal")
        {
            string title = parts[1];
            string desc = parts[2];
            int points = int.Parse(parts[3]);
            return new EternalGoal(title, desc, points);
        }
        else if (type == "Checklist")
        {
            string title = parts[1];
            string desc = parts[2];
            int points = int.Parse(parts[3]);
            int timesRequired = int.Parse(parts[4]);
            int timesCompleted = int.Parse(parts[5]);
            int bonus = int.Parse(parts[6]);
            var g = new ChecklistGoal(title, desc, points, timesRequired, bonus);
            g.SetCompletedFromLoad(timesCompleted);
            return g;
        }
        else
        {
            return null;
        }
    }
}

public class SimpleGoal : Goal
{
    private bool _isComplete;

    public SimpleGoal(string title, string desc, int points)
        : base(title, desc, points)
    {
        _isComplete = false;
    }

    public override int RecordEvent()
    {
        if (_isComplete) return 0;
        _isComplete = true;
        return GetBasePoints();
    }

    // Helper used when loading from file
    public void MarkCompleteFromLoad() => _isComplete = true;

    public override string GetStatus()
    {
        return _isComplete ? "[X]" : "[ ]";
    }

    public override string ToFileLine()
    {
        return $"Simple|{GetTitle()}|{GetDescription()}|{GetBasePoints()}|{_isComplete}";
    }
}

public class EternalGoal : Goal
{
    public EternalGoal(string title, string desc, int points)
        : base(title, desc, points)
    { }

    public override int RecordEvent()
    {
        // never complete; always award base points
        return GetBasePoints();
    }

    public override string GetStatus()
    {
        return "[∞]"; // represents eternal
    }

    public override string ToFileLine()
    {
        return $"Eternal|{GetTitle()}|{GetDescription()}|{GetBasePoints()}";
    }
}

public class ChecklistGoal : Goal
{
    private int _timesRequired;
    private int _timesCompleted;
    private int _completionBonus;
    private bool _completed;

    public ChecklistGoal(string title, string desc, int points, int timesRequired, int completionBonus)
        : base(title, desc, points)
    {
        _timesRequired = timesRequired;
        _timesCompleted = 0;
        _completionBonus = completionBonus;
        _completed = false;
    }

    public override int RecordEvent()
    {
        if (_completed) return 0;
        _timesCompleted++;
        int award = GetBasePoints();
        if (_timesCompleted >= _timesRequired)
        {
            _completed = true;
            award += _completionBonus;
        }
        return award;
    }

    // used when loading
    public void SetCompletedFromLoad(int timesCompleted)
    {
        _timesCompleted = timesCompleted;
        _completed = _timesCompleted >= _timesRequired;
    }

    public override string GetStatus()
    {
        return _completed ? $"[X] Completed {_timesCompleted}/{_timesRequired}" : $"[ ] Completed {_timesCompleted}/{_timesRequired}";
    }

    public override string ToFileLine()
    {
        return $"Checklist|{GetTitle()}|{GetDescription()}|{GetBasePoints()}|{_timesRequired}|{_timesCompleted}|{_completionBonus}";
    }
}

#endregion

#region Program / Manager

class Program
{
    static List<Goal> goals = new List<Goal>();
    static int score = 0;
    static List<string> badges = new List<string>();

    static void Main()
    {
        SeedExampleGoals();

        bool quit = false;
        while (!quit)
        {
            Console.WriteLine("\n=== Eternal Quest Menu ===");
            Console.WriteLine("1. Create a new goal");
            Console.WriteLine("2. List goals");
            Console.WriteLine("3. Record an event (complete a goal)");
            Console.WriteLine("4. Show score & badges");
            Console.WriteLine("5. Save goals to file");
            Console.WriteLine("6. Load goals from file");
            Console.WriteLine("7. Quit");
            Console.Write("Select an option: ");
            int choice = ReadIntInRange(1, 7);

            switch (choice)
            {
                case 1: CreateGoal(); break;
                case 2: ListGoals(); break;
                case 3: RecordEvent(); break;
                case 4: ShowScore(); break;
                case 5: SaveToFile(); break;
                case 6: LoadFromFile(); break;
                case 7: quit = true; break;
            }
        }
        Console.WriteLine("Goodbye!");
    }

    static void SeedExampleGoals()
    {
        goals.Add(new SimpleGoal("Run Marathon", "Finish a marathon", 1000));
        goals.Add(new EternalGoal("Read Scriptures", "Daily scripture study", 100));
        goals.Add(new ChecklistGoal("Attend Temple", "Go to the temple multiple times", 50, 3, 200)); // smaller numbers for demo
    }

    static void CreateGoal()
    {
        Console.WriteLine("\nChoose goal type:");
        Console.WriteLine("1. Simple Goal (single completion)");
        Console.WriteLine("2. Eternal Goal (repeatable)");
        Console.WriteLine("3. Checklist Goal (complete N times)");
        Console.Write("Type: ");
        int t = ReadIntInRange(1, 3);

        Console.Write("Enter title: ");
        string title = Console.ReadLine();
        Console.Write("Enter description: ");
        string desc = Console.ReadLine();
        Console.Write("Enter points awarded per completion (integer): ");
        int pts = ReadPositiveInt();

        if (t == 1)
        {
            goals.Add(new SimpleGoal(title, desc, pts));
        }
        else if (t == 2)
        {
            goals.Add(new EternalGoal(title, desc, pts));
        }
        else
        {
            Console.Write("Enter number of times required to complete: ");
            int times = ReadPositiveInt();
            Console.Write("Enter completion bonus points: ");
            int bonus = ReadPositiveInt();
            goals.Add(new ChecklistGoal(title, desc, pts, times, bonus));
        }

        Console.WriteLine("Goal created!");
    }

    static void ListGoals()
    {
        Console.WriteLine("\n--- Goals ---");
        if (!goals.Any())
        {
            Console.WriteLine("No goals found.");
            return;
        }

        for (int i = 0; i < goals.Count; i++)
        {
            var g = goals[i];
            Console.WriteLine($"{i + 1}. {g.GetStatus()} {g.GetTitle()} - {g.GetDescription()}");
        }
    }

    static void RecordEvent()
    {
        if (!goals.Any())
        {
            Console.WriteLine("No goals to record.");
            return;
        }

        ListGoals();
        Console.Write("Choose goal number to record: ");
        int idx = ReadIntInRange(1, goals.Count) - 1;
        var g = goals[idx];
        int awarded = g.RecordEvent();

        if (awarded == 0)
        {
            Console.WriteLine("This goal was already complete or no points awarded.");
        }
        else
        {
            score += awarded;
            Console.WriteLine($"You gained {awarded} points! Total score: {score}");
            // Extras: badges / level
            CheckForBadges(g);
            CheckLevelUp();
        }
    }

    static void ShowScore()
    {
        Console.WriteLine($"\nYour score: {score}");
        if (badges.Any())
        {
            Console.WriteLine("Badges earned:");
            foreach (var b in badges) Console.WriteLine($" - {b}");
        }
        else
        {
            Console.WriteLine("No badges yet.");
        }
    }

    static void SaveToFile()
    {
        Console.Write("Enter filename to save: ");
        string fname = Console.ReadLine();
        using (StreamWriter sw = new StreamWriter(fname))
        {
            sw.WriteLine(score);
            foreach (var g in goals)
            {
                sw.WriteLine(g.ToFileLine());
            }
            foreach (var b in badges)
            {
                sw.WriteLine($"BADGE|{b}");
            }
        }
        Console.WriteLine("Saved.");
    }

    static void LoadFromFile()
    {
        Console.Write("Enter filename to load: ");
        string fname = Console.ReadLine();
        if (!File.Exists(fname))
        {
            Console.WriteLine("File not found.");
            return;
        }

        var lines = File.ReadAllLines(fname);
        goals.Clear();
        badges.Clear();
        if (lines.Length == 0) return;

        score = int.Parse(lines[0]);
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            if (line.StartsWith("BADGE|"))
            {
                badges.Add(line.Substring(6));
            }
            else
            {
                var g = Goal.FromFileLine(line);
                if (g != null) goals.Add(g);
            }
        }
        Console.WriteLine("Loaded.");
    }

    static void CheckForBadges(Goal g)
    {
        // Award a simple badge when a checklist goal completes now
        if (g is ChecklistGoal)
        {
            var cg = g as ChecklistGoal;
            // If checklist is completed (status contains Completed and X/X where X>=required)
            string status = cg.GetStatus();
            if (status.Contains("Completed") && status.Contains("/"))
            {
                // if it is completed, award a badge if not already
                if (!badges.Contains($"Checklist Master: {cg.GetTitle()}"))
                {
                    badges.Add($"Checklist Master: {cg.GetTitle()}");
                    Console.WriteLine("Badge earned: Checklist Master!");
                }
            }
        }
    }

    static void CheckLevelUp()
    {
        int level = score / 1000; // every 1000 points is a new "level"
        if (level > 0)
        {
            Console.WriteLine($"You are level {level}! Keep going!");
        }
    }

    #region Helpers
    static int ReadIntInRange(int min, int max)
    {
        while (true)
        {
            string s = Console.ReadLine();
            if (int.TryParse(s, out int v) && v >= min && v <= max) return v;
            Console.Write($"Please enter an integer between {min} and {max}: ");
        }
    }

    static int ReadPositiveInt()
    {
        while (true)
        {
            string s = Console.ReadLine();
            if (int.TryParse(s, out int v) && v > 0) return v;
            Console.Write("Please enter a positive integer: ");
        }
    }
    #endregion
}

#endregion
