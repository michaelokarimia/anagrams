﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackerRank
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "BBDCEF";
            var searchString = "BBCDEF";

            int SCORE = 100;

            var sortedInput = getSortedString(input);

            var sortedSearchString = getSortedString(searchString);

            SCORE = GetScore(sortedInput, sortedSearchString, SCORE);

            if (input != searchString)
            {
                var swappingPenalty = calculateSwappingPenalty(input, searchString);
                SCORE-=swappingPenalty;
            }

            Console.WriteLine("Score is: {0}",SCORE);

            Console.ReadKey();

        }

        private static int calculateSwappingPenalty(string input, string searchString)
        {
            int requiredSwaps =0;
            //we know the two strings are anagrams of each length, iterate through through each and calculate penalty 
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] != searchString[i])
                {
                    requiredSwaps++;
                }
            }
            return (requiredSwaps/2)*5;
        }

        private static string getSortedString(string input)
        {
            var charArray = input.ToCharArray();
            Array.Sort(charArray);
            var sorted = "";

            foreach (char c in charArray)
            {
                sorted += c;
            }

            return sorted;
        }

        private static int GetScore(string input, string searchString, int currentScore)
        {
            int SCORE = currentScore;
            Trie mytrie = new Trie();

            mytrie.InsertRange(new List<string> {input});
          

            if (ContainsString(input,searchString))
            {
                return SCORE;
            }

            SCORE -= 5;
            //try next char
            if (searchString.Length > 0)
            {
                var chopTrailingCharOff = searchString.Substring(0, searchString.Length-1);
                
                return GetScore(input, chopTrailingCharOff, SCORE);
            }
            


            return SCORE;
        }

        private static bool ContainsString(string input, string searchString)
        {
            Trie mytrie = new Trie();

            mytrie.InsertRange(new List<string> { input });

            return mytrie.Search(searchString);
        }

        public class Trie
        {
            private readonly Node _root;

            public Trie()
            {
                _root = new Node('^', 0, null);
            }

            public Node Prefix(string s)
            {
                var currentNode = _root;
                var result = currentNode;

                foreach (var c in s)
                {
                    currentNode = currentNode.FindChildNode(c);
                    if (currentNode == null)
                        break;
                    result = currentNode;
                }

                return result;
            }

            public bool Search(string s)
            {
                var prefix = Prefix(s);
                return prefix.Depth == s.Length && prefix.FindChildNode('$') != null;
            }

            public void InsertRange(List<string> items)
            {
                for (int i = 0; i < items.Count; i++)
                    Insert(items[i]);
            }

            public void Insert(string s)
            {
                var commonPrefix = Prefix(s);
                var current = commonPrefix;

                for (var i = current.Depth; i < s.Length; i++)
                {
                    var newNode = new Node(s[i], current.Depth + 1, current);
                    current.Children.Add(newNode);
                    current = newNode;
                }

                current.Children.Add(new Node('$', current.Depth + 1, current));
            }
        }

        public class Node
        {
            public char Value { get; set; }
            public List<Node> Children { get; set; }
            public Node Parent { get; set; }
            public int Depth { get; set; }

            public Node(char value, int depth, Node parent)
            {
                Value = value;
                Children = new List<Node>();
                Depth = depth;
                Parent = parent;
            }

            public bool IsLeaf()
            {
                return Children.Count == 0;
            }

            public Node FindChildNode(char c)
            {
                foreach (var child in Children)
                    if (child.Value == c)
                        return child;

                return null;
            }
        }
    }
}
