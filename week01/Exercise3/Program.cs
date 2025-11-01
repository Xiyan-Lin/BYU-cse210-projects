using System;

class Program
{
    static void Main()
    {
        Random random = new Random();
        string playAgain;

        do
        {
            int magicNumber = random.Next(1, 101); // 產生 1 到 100 的亂數
            int guess = -1;
            int attempts = 0;

            Console.WriteLine("=== Guess My Number Game ===");
            Console.WriteLine("I have chosen a number between 1 and 100.");

            while (guess != magicNumber)
            {
                Console.Write("What is your guess? ");
                string input = Console.ReadLine();

                // 驗證輸入
                if (!int.TryParse(input, out guess))
                {
                    Console.WriteLine("Please enter a valid integer.");
                    continue;
                }

                attempts++;

                if (guess < magicNumber)
                {
                    Console.WriteLine("Higher");
                }
                else if (guess > magicNumber)
                {
                    Console.WriteLine("Lower");
                }
                else
                {
                    Console.WriteLine($"You guessed it in {attempts} tries!");
                }
            }

            Console.Write("Do you want to play again? (yes/no): ");
            playAgain = Console.ReadLine().Trim().ToLower();
            Console.WriteLine();

        } while (playAgain == "yes");

        Console.WriteLine("Thanks for playing! Goodbye.");
    }
}