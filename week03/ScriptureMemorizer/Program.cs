using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        /*
         * EXTRA FEATURES (exceed requirements)
         * -----------------------------------
         * 1. Scripture Library – randomly selects from multiple scriptures.
         * 2. Supports loading scriptures from a file "scriptures.txt".
         * 3. Smart hiding – only hides words not already hidden.
         */

        List<Scripture> library = LoadScriptureLibrary();

        // Randomly choose one scripture
        Random rand = new Random();
        Scripture scripture = library[rand.Next(library.Count)];

        string input = "";

        while (input.ToLower() != "quit" && !scripture.AllWordsHidden())
        {
            Console.Clear();
            scripture.Display();

            Console.WriteLine("\nPress ENTER to hide more words, or type 'quit' to exit:");
            input = Console.ReadLine();

            if (input.ToLower() != "quit")
                scripture.HideRandomWords();
        }

        Console.Clear();
        scripture.Display();
        Console.WriteLine("\nAll words are hidden. Program ending.\n");
    }

    static List<Scripture> LoadScriptureLibrary()
    {
        List<Scripture> scriptures = new List<Scripture>();

        // If no file exists, use built-in scriptures
        if (!File.Exists("scriptures.txt"))
        {
            scriptures.Add(new Scripture(
                new Reference("John", 3, 16),
                "For God so loved the world that he gave his only begotten Son"
            ));

            scriptures.Add(new Scripture(
                new Reference("Proverbs", 3, 5, 6),
                "Trust in the Lord with all thine heart and lean not unto thine own understanding"
            ));

            return scriptures;
        }

        // Otherwise load from file
        foreach (string line in File.ReadAllLines("scriptures.txt"))
        {
            // Format: Book|Chapter|VerseStart|VerseEnd|Text
            var parts = line.Split("|");
            string book = parts[0];
            int chapter = int.Parse(parts[1]);
            int start = int.Parse(parts[2]);
            int end = int.Parse(parts[3]);
            string text = parts[4];

            Reference r = start == end
                ? new Reference(book, chapter, start)
                : new Reference(book, chapter, start, end);

            scriptures.Add(new Scripture(r, text));
        }

        return scriptures;
    }
}
