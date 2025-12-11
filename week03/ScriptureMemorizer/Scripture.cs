using System;
using System.Collections.Generic;
using System.Linq;

public class Scripture
{
    private Reference _reference;
    private List<Word> _words;

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = text.Split(" ").Select(word => new Word(word)).ToList();
    }

    public void HideRandomWords(int count = 3)
    {
        Random rand = new Random();

        // Only hide words that are NOT hidden yet (Overachievement feature)
        var visibleWords = _words.Where(w => !w.IsHidden()).ToList();

        for (int i = 0; i < count && visibleWords.Count > 0; i++)
        {
            int index = rand.Next(visibleWords.Count);
            visibleWords[index].Hide();
            visibleWords.RemoveAt(index);
        }
    }

    public bool AllWordsHidden()
    {
        return _words.All(w => w.IsHidden());
    }

    public void Display()
    {
        Console.WriteLine(_reference.GetReferenceText());
        Console.WriteLine();
        Console.WriteLine(string.Join(" ", _words.Select(w => w.GetDisplayText())));
    }
}
