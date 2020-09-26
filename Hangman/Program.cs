using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Hangman
{
    public static class Program
    {
        private static int _lives = 9;
        private static string _gameMode = "";
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Animal Hangman by ajsprojects!");
            Console.Write("Please type your difficult ('easy','medium','hard'): ");
            _gameMode = Console.ReadLine().ToLower();
            Console.WriteLine("Game mode: " + _gameMode);
            var wordsToGuess = LoadWordsFromFile();
            Random rnd = new Random();
            int attempts = 0;
            var chosenWord = wordsToGuess[rnd.Next(0, wordsToGuess.Count)];
            var length = chosenWord.Length;
            StringBuilder chosenWordCopy = new StringBuilder(length);

            for (int i = 0; i < chosenWord.Length; i++)
            {
                if (chosenWord[i] != ' ')
                {
                    chosenWordCopy.Append('_');
                } else
                {
                    chosenWordCopy.Append(' ');
                }
            } 
            //Console.WriteLine(chosenWord + ", length: " + length);
            Console.WriteLine("Word: " + chosenWordCopy + " (" + length + ")");

            List<char> incorrectLetters = new List<char>();

            string input;
            char letter;

            while (CanContinue())
            {
                Console.Write("Enter your letter: ");
                input = Console.ReadLine().ToLower();
                letter = input[0];

                if (!IsValidInput(letter))
                    Console.WriteLine("Invalid input!");

                if (!chosenWord.Contains(letter, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine(letter + " is not in the word");
                    incorrectLetters.Add(letter);
                    attempts += 1;
                    _lives -= 1;
                }
                else
                {
                    int i = GetIndex(letter, chosenWord, chosenWordCopy);
                    if (i != -1)
                    {
                        chosenWordCopy.Remove(i, 1);
                        chosenWordCopy.Insert(i, letter);
                        Console.WriteLine("Correct guess well done!");
                    }
                }

                if (String.Equals(chosenWord, chosenWordCopy.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~");
                    Console.WriteLine("Word: " + chosenWordCopy + " (" + length + ")");
                    Console.WriteLine("Congratulations! You guessed the word with: " + attempts + " incorrect guesses!");
                    break;
                }

                if (incorrectLetters.Count > 0)
                {
                    Console.Write("Incorrect guesses: ");
                    foreach (char c in incorrectLetters)
                    {
                        Console.Write(c + ", ");
                    }

                }
                Console.WriteLine("\n");
                Console.WriteLine("Word: " + chosenWordCopy);
            }

        }
        static int GetIndex(char letter, string word, StringBuilder copyWord)
        {
            for (int i = 0; i < word.Length; i++)
            {
                char c = word[i];
                if (Char.ToLower(c) == letter && copyWord[i] == '_')
                {
                    return i;
                }
            }
            Console.WriteLine("You have already guessed this character!");
            return -1;
        }
        static bool IsValidInput(char c)
        {
            return Char.IsLetter(c);
        }
        static bool CanContinue()
        {
            if (_lives > 0)
            {
                Console.WriteLine("You have " + _lives + " live(s) remaining!");
                return true;
            }
            else
            {
                Console.WriteLine("You have 0 lives");
                Console.WriteLine("Game Over!");
                return false;
            }
        }
        static List<string> LoadWordsFromFile() 
        {
            var wordsToGuess = new List<string>();
            var path = Directory.GetParent(Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString());
            StreamReader file = new System.IO.StreamReader(path + "/Animals.txt");
            string line;
            if (path != null)
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (_gameMode == "easy" && line.Length <= 5)
                    {
                        wordsToGuess.Add(line);
                    } else if (_gameMode == "medium" && line.Length <= 9)
                    {
                        wordsToGuess.Add(line);
                    } else if (_gameMode == "hard")
                    {
                        wordsToGuess.Add(line);
                    }
                }
                file.Close();
            }
            return wordsToGuess;
        }

    }
}