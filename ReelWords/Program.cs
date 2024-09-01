using System;
using ReelWords.Core;

namespace ReelWords
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var state = Helpers.CreateGameState((int)DateTime.Now.Ticks);

            var view = new ConsoleView();
            
            var ctx = new GameContext()
            {
                State = state,
                GameView = view
            };
            
            ctx.StartGame();

            while (!state.IsEnded)
            {
                var input = Console.ReadLine();
                
                ctx.InputString = input;
                
                ctx.ProcessInput();

                // TODO:  Create simple unit tests to test your code in the ReelWordsTests project,
                // don't worry about creating tests for everything, just important functions as
                // seen for the Trie tests
            }
        }
    }
}