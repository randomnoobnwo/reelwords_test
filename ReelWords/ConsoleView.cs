using System;
using ReelWords.Core;

namespace ReelWords;

public class ConsoleView : IGameView
{
    public char[] Letters;
    public string Status;
    public int Score;

    public void Init(int reelsCount)
    {
        Letters = new char[reelsCount];
    }

    public void UpdateScore(int score)
    {
        Score = score;
        Render();
    }

    public void UpdateStatus(string status)
    {
        Status = status;
        Render();
    }
    
    public void EndGame(string message)
    {
        Console.Clear();
        Console.WriteLine(message);
        Console.WriteLine("Press any key to exit...");
        Console.ReadLine();
    }

    public void AnimateShift(int index, char letter)
    {
        Letters[index] = letter;
        Render();
    }

    public void Render()
    {
        Console.Clear();
        Console.WriteLine("---=== Reel Words ===---");
        Console.WriteLine("Help: R - reset, Q - quit");
        Console.WriteLine($"Score: {Score}");
        Console.WriteLine(Status);
        Console.WriteLine(string.Join(" ", Letters));
    }
}