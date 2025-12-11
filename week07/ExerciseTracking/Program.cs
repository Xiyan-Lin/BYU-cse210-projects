class Program
{
    static void Main(string[] args)
    {
        List<Activity> activities = new List<Activity>();

        // Create 3 activities
        activities.Add(new Running("03 Nov 2022", 30, 4.8));  // 4.8 km
        activities.Add(new Cycling("03 Nov 2022", 40, 15.0)); // 15 kph
        activities.Add(new Swimming("03 Nov 2022", 25, 20)); // 20 laps

        // Display all summaries
        foreach (Activity activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
        }
    }
}
