using System.Collections.Generic;
using System.Linq;
using ReelWords;
using ReelWords.Core;
using Xunit;

namespace ReelWordsTests;

public class StateTests
{
    [Fact]
    public void StartGameTest()
    {
        var reels = new[] { "abc", "def", "ghi" };
        var state = Helpers.CreateGameState(reels: reels);
        var ctx = new GameContext()
        {
            State = state,
            InputString = "j",
            GameView = new ConsoleView()
        };
        ctx.StartGame();
        Assert.True(reels[0][0] != state.Reels[0].First());
    }

    [Fact]
    public void TestShiftReel()
    {
        var state = new GameState()
        {
            Reels = new[]
            {
                new Queue<char>(new[] { 'a', 'b', 'c' })
            }
        };

        var ctx = new GameContext()
        {
            State = state
        };

        ctx.ShiftReel(0, 1);
        Assert.Equal('b', state.Reels[0].Peek());

        ctx.ShiftReel(0, 2);
        Assert.Equal('a', state.Reels[0].Peek());
    }


    [Fact]
    public void TestEmptyInput()
    {
        var state = Helpers.CreateGameState();
        var ctx = new GameContext()
        {
            State = state,
            InputString = "",
            GameView = new ConsoleView()
        };
        ctx.StartGame();
        var result = ctx.ValidateInput();
        Assert.False(result.IsValid);
    }

    private static bool[] GetValidCharsArray(char[] chars)
    {
        var validChars = new bool[26];
        foreach (var c in chars)
        {
            validChars[c - 'a'] = true;
        }

        return validChars;
    }

    [Fact]
    public void TestInvalidCharsInput()
    {
        var state = Helpers.CreateGameState(validChars: GetValidCharsArray(new[] { 'a', 'b', 'c' }));
        var ctx = new GameContext()
        {
            State = state,
            InputString = "j",
            GameView = new ConsoleView()
        };
        ctx.StartGame();
        var result = ctx.ValidateInput();
        Assert.False(result.IsValid);
    }

    [Fact]
    public void TestInputNotInLetters()
    {
        var state = Helpers.CreateGameState(reels: new[] { "a", "d", "g" });
        var ctx = new GameContext()
        {
            State = state,
            InputString = "j",
            GameView = new ConsoleView()
        };
        ctx.StartGame();
        var result = ctx.ValidateInput();
        Assert.False(result.IsValid);
    }

    [Fact]
    public void TestInputSuccess()
    {
        var state = Helpers.CreateGameState(reels: new[] { "d", "a", "d" });
        var ctx = new GameContext()
        {
            State = state,
            InputString = "dad",
            GameView = new ConsoleView()
        };
        ctx.StartGame();
        var result = ctx.ValidateInput();
        Assert.True(result.IsValid);
    }

    [Fact]
    public void TestShiftReels()
    {
        var state = Helpers.CreateGameState(reels: new[] { "abc", "def", "ghi" });
        var ctx = new GameContext()
        {
            State = state,
            GameView = new ConsoleView()
        };
        ctx.StartGame();
        var letters = state.CurrentLetters;
        ctx.InputString = string.Join("", letters);
        ctx.State.WordsTrie.Insert(ctx.InputString);
        ctx.ShiftReels();
        for (var i = 0; i < letters.Length; i++)
        {
            // current letter should not be the same as the previous one
            Assert.NotEqual(letters[i], ctx.State.CurrentLetters[i]);
            // previous letter should be at the end of queue
            Assert.Equal(letters[i], ctx.State.Reels[i].Last());
        }
    }
}