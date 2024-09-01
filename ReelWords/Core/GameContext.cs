using System;
using System.Collections.Generic;
using System.Linq;

namespace ReelWords.Core;

public class GameContext
{
    public GameState State { get; set; }
    public string InputString { get; set; }
    public IGameView GameView { get; set; }
}

public static class GameContextExtensions
{
    public static void ProcessInput(this GameContext ctx)
    {
        if (ctx.CheckControls()) return;
        
        var result = ctx.ValidateInput();
        
        if (result.IsValid)
        {
            ctx.ScorePoints();
            ctx.ShiftReels();
        }
        else
        {
            ctx.GameView?.UpdateStatus(result.Message);
        }
    }
    
    private static bool CheckControls(this GameContext ctx)
    {
        var input = ctx.InputString;
        switch (input.ToLowerInvariant())
        {
            case "r":
                ctx.Reset();
                return true;
            case "q":
                ctx.EndGame();
                return true;
            default:
                return false;
        }
    }
    
    private static void Reset(this GameContext ctx)
    {
        ctx.StartGame();
    }
    
    private static void EndGame(this GameContext ctx)
    {
        ctx.State.IsEnded = true;
        ctx.GameView?.EndGame("Game ended!");
    }

    public static void ShiftReel(this GameContext ctx, int index, int offset)
    {
        var reel = ctx.State.Reels[index];
        for (var i = 0; i < offset; i++)
        {
            var shifted = reel.Dequeue();
            reel.Enqueue(shifted);
            ctx.GameView?.AnimateShift(index, reel.Peek());
        }
    }
    
    public static void ShiftReels(this GameContext ctx)
    {
        var state = ctx.State;
        foreach (var c in ctx.InputString)
        {
            var index = Array.IndexOf(state.CurrentLetters, c);
            ctx.ShiftReel(index, 1);
        }
    }
    
    public static void ScorePoints(this GameContext ctx)
    {
        var pointsEarned = ctx.InputString.Select(c => ctx.State.ScorePoints[c - 'a']).ToArray();
        var points = pointsEarned.Sum();
        ctx.State.ScoredPoints += points;
        
        var pointsString = string.Join("+", pointsEarned);
        ctx.GameView?.UpdateStatus($"{pointsString}={points} scored for '{ctx.InputString}'");
        ctx.GameView?.UpdateScore(ctx.State.ScoredPoints);
    }

    public static InputResult ValidateInput(this GameContext ctx)
    {
        var state = ctx.State;
        var input = ctx.InputString;
        
        if (string.IsNullOrEmpty(input))
        {
            return new InputResult(false, "Input cannot be empty");
        }
        
        if (input.Any(c => !state.CurrentLetters.Contains(c)))
        {
            return new InputResult(false, "Input contains characters not in the current reels");
        }

        if (input.Any(c => !state.ValidChars[c - 'a']))
        {
            return new InputResult(false, "Input contains invalid characters");
        }
        
        // check if input is a word from Trie
        if (!state.WordsTrie.Search(input))
        {
            return new InputResult(false, "Input is not a valid word");
        }
        
        return new InputResult(true, "Word accepted!");
    }
    
    public static void StartGame(this GameContext ctx)
    {
        var state = ctx.State;
        var reelsCount = state.StartReels.Length;
        state.Reels = new Queue<char>[reelsCount];
        ctx.GameView?.Init(reelsCount);

        for (var i = 0; i < reelsCount; i++)
        {
            state.Reels[i] = new Queue<char>(state.StartReels[i]);

            var offset = state.Random.Next(1, state.StartReels[i].Length);
            ctx.ShiftReel(i, offset);
        }
        
        ctx.State.ScoredPoints = 0;
        ctx.GameView?.UpdateScore(ctx.State.ScoredPoints);
    }
}