using System;

class Program
{
    static void Main(string[] args)
    {
        Journal journal = new Journal();
        PromptGenerator promptGen = new PromptGenerator();

        int choice = 0;

        /*  
         * EXTRA FEATURES (exceed requirements)
         * -----------------------------------
         * - Added more prompts in PromptGenerator.
         * - Allows user to see current entry count.
         * - Cleaner formatting on display.
         */

        while (choice != 5)
        {
            Console.WriteLine("\nJournal Menu:");
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display journal");
            Console.WriteLine("3. Save journal to a file");
            Console.WriteLine("4. Load journal from a file");
            Console.WriteLine("5. Quit");
            Console.Write("Select an option: ");

            choice = int.Parse(Console.ReadLine());

            if (choice == 1)
            {
                string prompt = promptGen.GetRandomPrompt();
                Console.WriteLine($"\nPrompt: {prompt}");
                Console.Write("Your response: ");
                string response = Console.ReadLine();

                string date = DateTime.Now.ToShortDateString();

                Entry entry = new Entry(date, prompt, response);
                journal.AddEntry(entry);

                Console.WriteLine("\nEntry added!\n");
            }

            else if (choice == 2)
            {
                Console.WriteLine("\n--- Journal Entries ---\n");
                journal.DisplayJournal();
            }

            else if (choice == 3)
            {
                Console.Write("Enter filename to save: ");
                string filename = Console.ReadLine();
                journal.SaveToFile(filename);
                Console.WriteLine("Journal saved!\n");
            }

            else if (choice == 4)
            {
                Console.Write("Enter filename to load: ");
                string filename = Console.ReadLine();
                journal.LoadFromFile(filename);
                Console.WriteLine("Journal loaded!\n");
            }

            else if (choice == 5)
            {
                Console.WriteLine("Goodbye!");
            }
        }
    }
}
