using System;

class Program
{
    static void Main()
    {
        // 呼叫每個函式依序執行
        DisplayWelcome();

        string userName = PromptUserName();
        int userNumber = PromptUserNumber();
        int squaredNumber = SquareNumber(userNumber);

        DisplayResult(userName, squaredNumber);
    }

    // 顯示歡迎訊息
    static void DisplayWelcome()
    {
        Console.WriteLine("Welcome to the Program!");
    }

    // 詢問使用者姓名並回傳
    static string PromptUserName()
    {
        Console.Write("Please enter your name: ");
        return Console.ReadLine();
    }

    // 詢問使用者最喜歡的數字並回傳整數
    static int PromptUserNumber()
    {
        Console.Write("Please enter your favorite number: ");
        string input = Console.ReadLine();
        int number = int.Parse(input);
        return number;
    }

    // 回傳該數字的平方
    static int SquareNumber(int number)
    {
        return number * number;
    }

    // 顯示結果
    static void DisplayResult(string name, int square)
    {
        Console.WriteLine($"{name}, the square of your number is {square}");
    }
}