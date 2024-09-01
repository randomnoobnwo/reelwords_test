using System.Collections.Generic;

namespace ReelWords
{
    public class TrieNode
    {
        public Dictionary<char, TrieNode> Children { get; } = new();
        public bool IsEndOfWord { get; set; }
    }

    public class Trie
    {
        private readonly TrieNode _root = new();

        // Insert a word into the Trie
        public void Insert(string word)
        {
            var currentNode = _root;
            foreach (var ch in word)
            {
                if (!currentNode.Children.ContainsKey(ch))
                {
                    currentNode.Children[ch] = new TrieNode();
                }

                currentNode = currentNode.Children[ch];
            }

            currentNode.IsEndOfWord = true;
        }

        // Search for a word in the Trie
        public bool Search(string word)
        {
            var currentNode = _root;
            foreach (var ch in word)
            {
                if (!currentNode.Children.TryGetValue(ch, out var child))
                {
                    return false;
                }

                currentNode = child;
            }

            return currentNode.IsEndOfWord;
        }

        // Delete a word from the Trie
        public void Delete(string word)
        {
            DeleteHelper(_root, word, 0);
        }

        private static bool DeleteHelper(TrieNode currentNode, string word, int index)
        {
            if (index == word.Length)
            {
                if (!currentNode.IsEndOfWord)
                {
                    return false;
                }

                currentNode.IsEndOfWord = false;
                return currentNode.Children.Count == 0;
            }

            var ch = word[index];
            if (!currentNode.Children.TryGetValue(ch, out var child))
            {
                return false;
            }

            var shouldDeleteChildNode = DeleteHelper(child, word, index + 1);

            if (!shouldDeleteChildNode) return false;
            
            currentNode.Children.Remove(ch);
            return !currentNode.IsEndOfWord && currentNode.Children.Count == 0;
        }
    }
}