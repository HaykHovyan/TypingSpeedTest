namespace TypingSpeedTest
{
    public class Game
    {
        private int duration;
        private int durationSeconds;
        private string text;
        private int index;
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
            index = 0;
            timer = new System.Timers.Timer(duration);
            timeRemaining = new System.Timers.Timer(1000);
            result = new Result();
        }

        /// <summary>
        /// Called once at the start of the game
        /// </summary>
        public async Task Start()
        {
            await GUI.DisplayCountdown();
            GUI.Setup();
            GenerateText(text);
            timer.Elapsed += async (s, e) => await End();
            timer.Start();
            timeRemaining.Elapsed += async (s, e) =>
            {
                durationSeconds--;
                await GUI.DisplayRemainingTime(durationSeconds);
            };
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
            else if (index == text.Length - 1)
            {
                await End();
                return;
            }

            char inputChar = InputOutput.Read();
            CheckInput(inputChar, text[index]);
        }

        /// <summary>
        /// Called either when the game timer counts down to 0 or when the end of the text is reached
        /// </summary>
        private async Task End()
        {
            timer.Stop();
            timeRemaining.Stop();
            await CalculateWPM();
            await GUI.DisplayResults(result.GrossWPM, result.NetWPM, result.Mistakes.Count());
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
        /// Checks player input against the letter at the current index in the text
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
            GUI.MarkCorrect(text[index]);
            index++;
            result.CharactersTyped++;
        }

        /// <summary>
        /// Called when the player inputs an incorrect letter
        /// </summary>
        private void MarkIncorrect()
        {

            GUI.MarkIncorrect(text[index]);
            index++;
            result.CharactersTyped++;
            result.Mistakes.Add(text[index]);
        }

        /// <summary>
        /// Called when the player presses backspace
        /// </summary>
        private void MarkBackSpace()
        {
            if (index == 0)
            {
                return;
            }
            result.CharactersTyped--;
            index--;
            GUI.BackSpace(text[index]);
            if (result.Mistakes.Count() != 0 && result.Mistakes.Contains(text[index]))
            {
                result.Mistakes.RemoveAt(result.Mistakes.LastIndexOf(text[index]));
            }
        }

        /// <summary>
        /// Called at the end of the game to calculate player statistics
        /// </summary>
        private Task CalculateWPM()
        {
            const int wordLength = 5;
            int textLength = text.Length;
            int durationMinutes = Math.Clamp(duration / 60_000, 1, 2);
            result.GrossWPM = (result.CharactersTyped / wordLength) / durationMinutes;
            result.NetWPM = result.GrossWPM - (result.Mistakes.Count() / durationMinutes);
            return Task.CompletedTask;
        }
    }
}