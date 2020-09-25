using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Dynamic;

namespace Hangman
{
    public static class Program
    {
        static int lives = 9;
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Animal Hangman by ajsprojects!");
            var wordsToGuess = new List<string>() { "bird", "dog", "rabbit", "otter", "penguin", "mouse", "giraffe", "panda", "tiger", "squirrel",
                "elephant", "lion", "anteater", "bobcat", "horse", "chimpanzee", "goose", "caterpillar", "kangaroo", "moose", "meercat"};
            Random rnd = new Random();
            int attempts = 0;
            var chosenWord = wordsToGuess[rnd.Next(0, wordsToGuess.Count)];
            var length = chosenWord.Length;
            StringBuilder chosenWordCopy = new StringBuilder(length);

            for (int i = 0; i < chosenWord.Length; i++)
                chosenWordCopy.Append('_');

            //Console.WriteLine(chosenWord + " , length: " + length);
            Console.WriteLine("Word: " + chosenWordCopy + " (" + length + ")");

            List<char> incorrectLetters = new List<char>();

            string input;
            char letter;

            while (canContinue())
            {
                Console.Write("Enter your letter: ");
                input = Console.ReadLine().ToLower();
                letter = input[0];

                if (!isValidInput(letter))
                    Console.WriteLine("Invalid input!");

                if (!chosenWord.Contains(letter))
                {
                    Console.WriteLine(letter + " is not in word!");
                    incorrectLetters.Add(letter);
                    attempts += 1;
                    lives -= 1;
                }
                else
                {
                    int i = getIndex(letter, chosenWord, chosenWordCopy);
                    chosenWordCopy.Remove(i, 1);
                    chosenWordCopy.Insert(i, letter);
                    Console.WriteLine("Correct guess well done!");
                }

                if (chosenWordCopy.Equals(chosenWord))
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

        static int getIndex(char letter, string word, StringBuilder copyWord)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == letter && copyWord[i] == '_')
                {
                    return i;
                }
            }
            return 0;
        }

        static bool isValidInput(char c)
        {
            return Char.IsLetter(c);
        }

        static bool canContinue()
        {
            if (lives > 0)
            {
                Console.WriteLine("You have " + lives + " live(s) remaining!");
                return true;
            }
            else
            {
                Console.WriteLine("You have 0 lives");
                Console.WriteLine("Game Over!");
                return false;
            }
        }
    }
}