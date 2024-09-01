using System;
using System.Collections.Generic;
using System.Linq;

namespace ReelWords.Core;

public class GameState
{
    public Queue<char>[] Reels;
    public Trie WordsTrie;
    public Random Random;
    public string[] StartReels;
    public bool[] ValidChars;
    public int ScoredPoints { get; set; }
    public bool IsEnded;
    public int[] ScorePoints;
    public char[] CurrentLetters => Reels.Select(r => r.Peek()).ToArray();
}