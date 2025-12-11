public class Entry
{
    public string _date;
    public string _prompt;
    public string _response;

    public Entry(string date, string prompt, string response)
    {
        _date = date;
        _prompt = prompt;
        _response = response;
    }

    public string GetDisplayString()
    {
        return $"{_date} | Prompt: {_prompt}\nResponse: {_response}\n";
    }

    public string ToFileFormat()
    {
        // Using ~|~ as separator because it's unlikely to appear in text
        return $"{_date}~|~{_prompt}~|~{_response}";
    }

    public static Entry FromFileFormat(string line)
    {
        string[] parts = line.Split("~|~");
        return new Entry(parts[0], parts[1], parts[2]);
    }
}
