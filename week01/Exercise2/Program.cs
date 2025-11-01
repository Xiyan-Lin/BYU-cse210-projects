using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Enter your grade percentage (0-100): ");
        string input = Console.ReadLine();
        if (!int.TryParse(input, out int grade))
        {
            Console.WriteLine("Invalid input. Please enter an integer.");
            return;
        }

        // Clamp to valid range 0..100
        if (grade < 0) grade = 0;
        if (grade > 100) grade = 100;

        // Determine letter
        string letter;
        if (grade >= 90) letter = "A";
        else if (grade >= 80) letter = "B";
        else if (grade >= 70) letter = "C";
        else if (grade >= 60) letter = "D";
        else letter = "F";

        // Determine sign (+ / - / "")
        string sign = "";
        if (letter != "F") // F has no sign
        {
            int lastDigit = grade % 10;
            if (letter == "A")
            {
                // No A+, only A or A-
                if (lastDigit < 3) sign = "-";
            }
            else
            {
                if (lastDigit >= 7) sign = "+";
                else if (lastDigit < 3) sign = "-";
            }
        }

        // Single print of the full letter grade
        Console.WriteLine($"Your letter grade is {letter}{sign}.");

        // Separate pass/fail message (pass if >= 70)
        if (grade >= 70)
            Console.WriteLine("Congratulations — you passed the course!");
        else
            Console.WriteLine("Keep trying — you can improve next time!");
    }
}