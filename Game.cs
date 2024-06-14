using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static TypeSpeedTest.Game;

namespace TypeSpeedTest
{
    public class Game : IDisposable
    {
        int duration;
        string text;
        char[] letters;
        int index;
        System.Timers.Timer timer;
        List<char> mistakes;

        struct Result
        {
            public static int charactersTyped;
            public static int grossWPM;
            public static int mistakes;
            public static int netWPM;
        }

        public void Dispose()
        {

        }

        public Game()
        {
            duration = 60_000; //milliseconds
            timer = new System.Timers.Timer(duration);
            text = Texts.GetText();
            letters = text.ToCharArray();
            index = 0;
            mistakes = new List<char>();
            Result.grossWPM = 0;
            Result.mistakes = 0;
            Result.netWPM = 0;
        }

        public async Task Start()
        {
            await GUI.StartGame();
            await GUI.DisplayCountdown();
            GenerateText(text);

            timer.Elapsed += async (s, e) => await End();
            timer.Start();
        }

        public async Task Update()
        {
            if (timer.Enabled == true && index != letters.Length - 1)
            {
                try
                {
                    char inputChar = InputOutput.Read();
                    if (inputChar != '\0')
                    {
                        var checkResult = CheckInput(inputChar, letters[index]);
                        switch (checkResult)
                        {
                            //correct
                            case 1:
                                GUI.MarkCorrect(letters[index]);
                                index++;
                                Result.charactersTyped++;
                                break;
                            //incorrect
                            case -1:
                                GUI.MarkIncorrect(letters[index]);
                                mistakes.Add(letters[index]);
                                index++;
                                Result.charactersTyped++;
                                break;
                            //backspace
                            case 0:
                                if (index == 0)
                                {
                                    break;
                                }
                                Result.charactersTyped--;
                                index--;
                                GUI.BackSpace(letters[index]);
                                if (mistakes.Count() != 0 && mistakes.Contains(letters[index]))
                                {
                                    mistakes.RemoveAt(mistakes.LastIndexOf(letters[index]));
                                }
                                break;
                            default:
                                break;
                        };
                    }
                }
                catch (Exception ex)
                {
                    await GUI.DisplayError(ex);
                    Console.ReadLine();
                    Environment.Exit(0);
                }

            }
            if (index == letters.Length - 1)
            {
                timer.Stop();
                await End();
            }
        }

        async Task End()
        {
            {
                timer.Stop();
                await CalculateWPM();
                await GUI.DisplayResults(Result.grossWPM, Result.netWPM, Result.mistakes);
                var restart = await GUI.PromptRestart();
                if (restart == true)
                {
                    Program.Main();
                    this.Dispose();
                }
            };
        }

        void GenerateText(string text)
        {
            Console.WriteLine(text);
            Console.SetCursorPosition(0, 0);
        }

        public static int CheckInput(char input, char letter)
        {
            if (input == letter)
            {
                return 1;
            }
            else if (input == '\b')
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        Task CalculateWPM()
        {
            const int wordLength = 5;
            int textLength = text.Length;
            int durationMinutes = Math.Clamp(duration / 60_000, 1, 2);
            Result.grossWPM = (Result.charactersTyped / wordLength) / durationMinutes;
            Result.mistakes = mistakes.Count();
            Result.netWPM = Result.grossWPM - (Result.mistakes / durationMinutes);
            return Task.CompletedTask;
        }
    }
}