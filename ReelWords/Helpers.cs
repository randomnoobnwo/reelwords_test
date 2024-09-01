using System;
using System.Collections.Generic;
using System.Linq;
using ReelWords.Core;

namespace ReelWords;

public static class Helpers
{
    public static GameState CreateGameState(int? randomSeed = null, string[] reels = null, Trie words = null, bool[] validChars = null)
    {
        randomSeed ??= (int)DateTime.Now.Ticks;
        reels ??= LoadReels("Resources/reels.txt");
        words ??= new Trie();
        validChars ??= GetValidCharsArray(reels);
        var scores = LoadScores("Resources/scores.txt");
        
        ProcessWords("Resources/american-english-large.txt", word =>
        {
            if (IsValid(validChars, word))
            {
                words.Insert(word);
            }
        });

        return new GameState()
        {
            Random = new Random(randomSeed.GetValueOrDefault()),
            StartReels = reels,
            WordsTrie = words,
            ValidChars = validChars,
            ScorePoints = scores
        };
    }

    private static string[] LoadReels(string inputFile)
    {
        var file = System.IO.File.OpenRead(inputFile);
        var reader = new System.IO.StreamReader(file);

        var result = new List<string>();
        while (!reader.EndOfStream)
        {
            var reel = reader.ReadLine();
            result.Add(string.Join(null, reel.Split(' ')));
        }

        return result.ToArray();
    }

    private static bool[] GetValidCharsArray(string[] reels)
    {
        var validChars = new List<char>();
        foreach (var reel in reels)
        {
            validChars.AddRange(reel.ToCharArray());
        }

        var validSet = new bool[26];

        foreach (var c in validChars.Distinct().OrderBy(c => c))
        {
            validSet[c - 'a'] = true;
        }

        return validSet;
    }

    private static bool IsValid(bool[] validSet, string word)
    {
        foreach (var cIdx in word.Select(c => c - 'a'))
        {
            // non alphabet character
            if (cIdx is < 0 or >= 26)
            {
                return false;
            }
            
            // not in valid set
            if (!validSet[cIdx])
            {
                return false;
            }
        }

        return true;
    }

    private static int[] LoadScores(string inputFile)
    {
        var scores = System.IO.File.ReadAllLines(inputFile);
        return scores.Select(s => int.Parse(s.Split(" ")[1])).ToArray();
    }

    private static void ProcessWords(string path, Action<string> act)
    {
        var file = System.IO.File.OpenRead(path);
        var reader = new System.IO.StreamReader(file);

        while (!reader.EndOfStream)
        {
            var word = reader.ReadLine();
            act?.Invoke(word?.ToLowerInvariant());
        }
    }
}