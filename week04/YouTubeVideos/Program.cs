using System;
using System.Collections.Generic;

// -------------------- Comment Class --------------------
public class Comment
{
    public string CommenterName { get; set; }
    public string Text { get; set; }

    public Comment(string name, string text)
    {
        CommenterName = name;
        Text = text;
    }
}

// -------------------- Video Class --------------------
public class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int LengthSeconds { get; set; }

    private List<Comment> _comments = new List<Comment>();

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }

    public int GetNumberOfComments()
    {
        return _comments.Count;
    }

    public List<Comment> GetComments()
    {
        return _comments;
    }
}

// -------------------- Main Program --------------------
class Program
{
    static void Main(string[] args)
    {
        // 建立影片清單
        List<Video> videos = new List<Video>();

        // -------------------- Video 1 --------------------
        Video v1 = new Video()
        {
            Title = "How to Cook Pasta",
            Author = "Chef Mario",
            LengthSeconds = 300
        };
        v1.AddComment(new Comment("John", "Great tutorial!"));
        v1.AddComment(new Comment("Emma", "I tried this and loved it."));
        v1.AddComment(new Comment("Lucas", "Easy to follow, thanks!"));
        videos.Add(v1);

        // -------------------- Video 2 --------------------
        Video v2 = new Video()
        {
            Title = "C# Classes Explained",
            Author = "Tech Academy",
            LengthSeconds = 480
        };
        v2.AddComment(new Comment("Alice", "Very clear explanation."));
        v2.AddComment(new Comment("Bob", "Helped me a lot, thank you!"));
        v2.AddComment(new Comment("Sam", "Great examples."));
        videos.Add(v2);

        // -------------------- Video 3 --------------------
        Video v3 = new Video()
        {
            Title = "Travel Vlog: Japan",
            Author = "TravelWithMe",
            LengthSeconds = 900
        };
        v3.AddComment(new Comment("Kevin", "Japan looks amazing!"));
        v3.AddComment(new Comment("Hannah", "Love this vlog!"));
        v3.AddComment(new Comment("Mike", "Hope to visit one day."));
        videos.Add(v3);

        // -------------------- Display All --------------------
        foreach (Video video in videos)
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine($"Title: {video.Title}");
            Console.WriteLine($"Author: {video.Author}");
            Console.WriteLine($"Length: {video.LengthSeconds} seconds");
            Console.WriteLine($"Number of Comments: {video.GetNumberOfComments()}");

            Console.WriteLine("Comments:");
            foreach (Comment c in video.GetComments())
            {
                Console.WriteLine($"  - {c.CommenterName}: {c.Text}");
            }
            Console.WriteLine();
        }
    }
}
