﻿namespace TypingSpeedTest
{
    public class Game
    {
        private int duration;
        private int durationSeconds;
        private string text;
        private int currentIndex;
        private System.Timers.Timer timer;
        private System.Timers.Timer timeRemaining;
        private Result result;

        /// <summary>
        /// Standard ctor for variable initialization
        /// </summary>
        public Game()
        {
            duration = 60_000;
            durationSeconds = duration / 1000;
            text = Texts.GetText();
            currentIndex = 0;
            timer = new System.Timers.Timer(duration);
            timer.Elapsed += async (s, e) => await End();
            timeRemaining = new System.Timers.Timer(1000);
            timeRemaining.Elapsed += async (s, e) =>
            {
                durationSeconds--;
                await GUI.DisplayRemainingTime(durationSeconds);
            };
            result = new Result();
        }

        /// <summary>
        /// Called once at the start of the game
        /// </summary>
        public async Task Start()
        {
            await GUI.DisplayCountdown();
            GUI.SetupGameScreen();
            GenerateText(text);
            
            timer.Start();
            timeRemaining.Start();
        }

        /// <summary>
        /// Called every second until the process is terminated
        /// </summary>
        /// <returns>
        /// A boolean indicating whether the game is finished
        /// </returns>
        public async Task Update()
        {
            if (timer.Enabled == false)
            {
                return;
            }
            else if (currentIndex == text.Length - 1)
            {
                await End();
                return;
            }

            char inputChar = InputOutput.Read();
            CheckInput(inputChar, text[currentIndex]);
        }

        /// <summary>
        /// Called either when the game timer counts down to 0 or when the end of the text is reached
        /// </summary>
        private async Task End()
        {
            timer.Stop();
            timeRemaining.Stop();
            await CalculateWPM();
            await GUI.DisplayResults(result);
            bool restart = await GUI.PromptRestart();
            if (restart)
            {
                Program.Main();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Generates the text to be typed
        /// </summary>
        /// <param name="text">
        /// The text to be typed, passed in as a single string variable
        /// </param>
        private void GenerateText(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (Console.CursorLeft == Console.WindowWidth - 15)
                {
                    Console.WriteLine();
                }
                Console.Write(text[i]);
            }
            Console.SetCursorPosition(0, 0);
        }

        /// <summary>
        /// Checks player input against the letter at the current currentIndex in the text
        /// </summary>
        private void CheckInput(char input, char currentLetter)
        {
            if (input == currentLetter)
            {
                MarkCorrect();
            }
            else if (input == '\b')
            {
                MarkBackSpace();
            }
            else if (input == '\0')
            {
                return;
            }
            else
            {
                MarkIncorrect();
            }
        }

        /// <summary>
        /// Called when the player inputs the correct letter
        /// </summary>
        private void MarkCorrect()
        {
            GUI.MarkCorrect(text[currentIndex]);
            currentIndex++;
            result.CharactersTyped++;
        }

        /// <summary>
        /// Called when the player inputs an incorrect letter
        /// </summary>
        private void MarkIncorrect()
        {

            GUI.MarkIncorrect(text[currentIndex]);
            currentIndex++;
            result.CharactersTyped++;
            result.Mistakes.Add(text[currentIndex]);
        }

        /// <summary>
        /// Called when the player presses backspace
        /// </summary>
        private void MarkBackSpace()
        {
            if (currentIndex == 0)
            {
                return;
            }
            result.CharactersTyped--;
            currentIndex--;
            GUI.BackSpace(text[currentIndex]);
            if (result.Mistakes.Count() != 0 && result.Mistakes.Contains(text[currentIndex]))
            {
                result.Mistakes.RemoveAt(result.Mistakes.LastIndexOf(text[currentIndex]));
            }
        }

        /// <summary>
        /// Called at the end of the game to calculate player statistics
        /// </summary>
        private Task CalculateWPM()
        {
            const int wordLength = 5;
            int textLength = text.Length;
            float durationMinutes = duration / 60_000;
            result.GrossWPM = (result.CharactersTyped / wordLength) / durationMinutes;
            result.NetWPM = result.GrossWPM - (result.Mistakes.Count() / durationMinutes);
            return Task.CompletedTask;
        }
    }
}