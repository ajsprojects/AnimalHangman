using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace Hangman
{
    public static class Program
    {
        private static int _lives;
        private static string _chosenWord;
        private static GameDifficulty _gameDifficulty;
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Animal Hangman by ajsprojects!");
            SetupGame();
        }
        private static void PlayGame()
        {
            int attempts = 0;
            var length = _chosenWord.Length;
            StringBuilder chosenWordCopy = new StringBuilder(length);

            for (int i = 0; i < _chosenWord.Length; i++)
            {
                if (_chosenWord[i] != ' ')
                {
                    chosenWordCopy.Append('_');
                }
                else
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

                if (!_chosenWord.Contains(letter, StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine(letter + " is not in the word");
                    incorrectLetters.Add(letter);
                    attempts += 1;
                    _lives -= 1;
                }
                else
                {
                    int i = GetIndex(letter, _chosenWord, chosenWordCopy);
                    if (i != -1)
                    {
                        chosenWordCopy.Remove(i, 1);
                        chosenWordCopy.Insert(i, letter);
                        Console.WriteLine("Correct guess well done!");
                    }
                }

                if (String.Equals(_chosenWord, chosenWordCopy.ToString(), StringComparison.OrdinalIgnoreCase))
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
            Restart();
        }
        private static void Restart()
        {
            Console.WriteLine("Restart game? Press spacebar to restart \n");
            if(Console.ReadKey(true).Key == ConsoleKey.Spacebar)
            {
                SetupGame();
            }
        }
        private static void SetupGame()
        {
            try
            {
                Console.Write("Please enter your difficulty 1,2,3 (easy = 1, medium = 2, hard = 3): ");
                int userSelectedDifficulty = Convert.ToInt32(Console.ReadLine());
                _gameDifficulty = (GameDifficulty) userSelectedDifficulty;
                SetLives();
                var wordsToGuess = LoadWordsFromFile();
                Console.WriteLine("Game mode: " + _gameDifficulty);
                Random rnd = new Random();
                _chosenWord = wordsToGuess[rnd.Next(0, wordsToGuess.Count)];
                PlayGame();
            } catch
            {
                Console.WriteLine("Invalid user difficulty selected! \n");
                SetupGame();
            } 
        }
        private static int GetIndex(char letter, string word, StringBuilder copyWord)
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
        private static bool IsValidInput(char c)
        {
            return Char.IsLetter(c);
        }
        private static bool CanContinue()
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
        private static List<string> LoadWordsFromFile() 
        {
            var wordsToGuess = new List<string>();
            var path = Directory.GetParent(Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString());
            StreamReader file = new StreamReader(path + "/Animals.txt");
            string line;
            if (path != null)
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (_gameDifficulty == GameDifficulty.Easy && line.Length <= 5)
                    {
                        wordsToGuess.Add(line);
                    } else if (_gameDifficulty == GameDifficulty.Medium && line.Length <= 9)
                    {
                        wordsToGuess.Add(line);
                    } else if (_gameDifficulty == GameDifficulty.Hard && line.Length >= 10)
                    {
                        wordsToGuess.Add(line);
                    }
                }
                file.Close();
            }
            return wordsToGuess;
        }

        private static void SetLives()
        {
            if (_gameDifficulty == GameDifficulty.Easy)
            {
                _lives = 11;
            }
            else if (_gameDifficulty == GameDifficulty.Medium)
            {
                _lives = 9;
            }
            else if (_gameDifficulty == GameDifficulty.Hard)
            {
                _lives = 7;
            }
        }
    }
}