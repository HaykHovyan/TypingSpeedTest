using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TypeSpeedTest
{
    public static class GUI
    {
        public static void Initialize()
        {
            Console.Title = "Typing Speed Test";
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
        }

        public static Task Clear()
        {
            Console.Clear();
            return Task.CompletedTask;
        }

        public static void HideCursor()
        {
            Console.CursorVisible = false;
        }

        public static void ShowCursor()
        {
            Console.CursorVisible = true;
        }

        public static async Task StartGame()
        {
            await InputOutput.Write("Get Ready!");
            await Task.Delay(1000);
            Console.Clear();
        }

        public async static Task DisplayCountdown()
        {
            HideCursor();
            int countDown = 3;
            for (int i = 0; i < countDown; i++)
            {
                await InputOutput.Write((countDown - i).ToString());
                Console.Beep();
                Console.CursorLeft = Console.WindowWidth / 2;
                //Thread.Sleep(1000);
                await Task.Delay(1000);
            }
            Console.Clear();
            ShowCursor();
        }

        public static async Task DisplayResults(int grossWPM, int netWPM, int mistakes)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            await InputOutput.Write("Well Done!");
            await Task.Delay(1000);
            await InputOutput.Write("Here are your stats\n", 0, Console.CursorTop + 1);
            await InputOutput.Write("Gross WPM: " + grossWPM, 0, Console.CursorTop + 1);
            await InputOutput.Write("Mistakes: " + mistakes, 0, Console.CursorTop + 1);
            if (netWPM >= 0)
            {
                await InputOutput.Write("Net WPM: " + netWPM, 0, Console.CursorTop + 1);
            }
            else
            {
                await InputOutput.Write("Net WPM: Less than 0", 0, Console.CursorTop + 1);
            }
            await Task.Delay(1000);
            await InputOutput.Write($"\n\n\n", 0, Console.CursorTop + 1);
        }

        public static async Task<bool> PromptRestart()
        {
            await InputOutput.Write("Press Space to go again...", 0, Console.CursorTop + 1);
            //if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Spacebar)
            //{
            //    return true;
            //}
            while (!Console.KeyAvailable)
            {
                await Task.Delay(10);
            }
            if (Console.ReadKey(true).Key == ConsoleKey.Spacebar)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void MarkCorrect(char letter)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write(letter);
            if (Console.CursorLeft == Console.WindowWidth - 1)
                Console.WriteLine();
        }

        public static void MarkIncorrect(char letter)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(letter);
            if (Console.CursorLeft == Console.WindowWidth - 1)
                Console.WriteLine();
        }

        public static void BackSpace(char letter)
        {
            if (Console.CursorLeft == 0 && Console.CursorTop == 0)
            {
                return;
            }
            else if (Console.CursorLeft == 0)
            {
                Console.CursorTop--;
                Console.CursorLeft = Console.WindowWidth - 2;

            }
            else
            {
                Console.CursorLeft--;
            }
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write(letter);
            Console.CursorLeft--;
        }

        public static async Task DisplayError(Exception error)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();

            await InputOutput.Write("The Program Encountered and Error!\n\n\n" + error.Message);
        }
    }
}