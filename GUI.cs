namespace TypingSpeedTest
{
    public static class GUI
    {
        private static readonly ConsoleColor defaultBackground = ConsoleColor.White;
        private static readonly ConsoleColor defaultForeground = ConsoleColor.Black;
        private static readonly object syncLock = new object();

        public static void Clear()
        {
            Console.Clear();
        }

        public static void HideCursor()
        {
            Console.CursorVisible = false;
        }

        public static void ShowCursor()
        {
            Console.CursorVisible = true;
        }

        /// <summary>
        /// Sets the window title, icon, and background and foreground colors
        /// </summary>
        public static void Initialize()
        {
            Console.Title = "Typing Speed Test";
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                System.Drawing.Icon icon = new System.Drawing.Icon("window.ico");
                SetWindowIcon(icon);
            }
            Console.BackgroundColor = defaultBackground;
            Console.ForegroundColor = defaultForeground;
            Clear();
        }

        public static async Task PromptStart()
        {
            await InputOutput.WritePretty("Press Space to begin...");
            while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Spacebar)
            {
                await Task.Delay(10);
            }
        }

        /// <summary>
        /// Sets the text screen border as well as additional text in the secondary window
        /// </summary>
        public static void SetupGameScreen()
        {
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth - 15, i);
                Console.Write(" | ");
            }
            Console.SetCursorPosition(Console.WindowWidth - 8, 0);
            Console.Write("Time");
            Console.SetCursorPosition(Console.WindowWidth - 7, 1);
            Console.Write(60);
            Console.SetCursorPosition(0, 0);
        }

        /// <summary>
        /// Displays the initial countdown before the game starts
        /// </summary>
        public async static Task DisplayCountdown()
        {
            Clear();
            await InputOutput.WritePretty("Get Ready!");
            await Task.Delay(2000);
            Clear();
            HideCursor();
            int countDown = 3;
            for (int i = countDown; i > 0; i--)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
                Console.Write(i);
                Console.Beep();
                await Task.Delay(1000);
            }
            Clear();
            ShowCursor();
        }

        /// <summary>
        /// Displays the time remaining
        /// </summary>
        /// <param name="timeRemaining"></param>
        public static Task DisplayRemainingTime(int timeRemaining)
        {
            lock (syncLock)
            {
                var previousPosition = Console.GetCursorPosition();
                Console.BackgroundColor = defaultBackground;
                Console.SetCursorPosition(Console.WindowWidth - 7, 1);
                Console.Write(timeRemaining);
                if (timeRemaining == 9)
                {
                    Console.Write(" ");
                }
                Console.SetCursorPosition(previousPosition.Left, previousPosition.Top);
            }
            Thread.Sleep(100);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called if the user inputs the correct letter
        /// </summary>
        /// <param name="letter"></param>
        public static void MarkCorrect(char letter)
        {
            lock (syncLock)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write(letter);
                if (Console.CursorLeft == Console.WindowWidth - 15)
                {
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Called if the user inputs an incorrect letter
        /// </summary>
        /// <param name="letter"></param>
        public static void MarkIncorrect(char letter)
        {
            lock (syncLock)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(letter);
                if (Console.CursorLeft == Console.WindowWidth - 15)
                {
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// called if the user presses backspace
        /// </summary>
        /// <param name="letter"></param>
        public static void BackSpace(char letter)
        {
            lock (syncLock)
            {
                if (Console.CursorLeft == 0 && Console.CursorTop == 0)
                {
                    return;
                }
                else if (Console.CursorLeft == 0)
                {
                    Console.CursorTop--;
                    Console.CursorLeft = Console.WindowWidth - 16;
                }
                else
                {
                    Console.CursorLeft--;
                }
                Console.BackgroundColor = defaultBackground;
                Console.Write(letter);
                Console.CursorLeft--;
            }
        }

        /// <summary>
        /// Displays the final results
        /// </summary>
        /// <param name="grossWPM"></param>
        /// <param name="netWPM"></param>
        /// <param name="mistakes"></param>
        /// <returns></returns>
        public static async Task DisplayResults(Result result)
        {
            Console.ForegroundColor = defaultForeground;
            Console.BackgroundColor = defaultBackground;
            Clear();
            await InputOutput.WritePretty("Well Done!");
            await Task.Delay(1000);
            await InputOutput.WritePretty("\nHere are your stats\n" +
                                          "Gross WPM: " + (int)result.GrossWPM + "\n" +
                                          "Mistakes: " + result.Mistakes.Count, null, Console.CursorTop + 1);
            if (result.NetWPM >= 0)
            {
                await InputOutput.WritePretty("Net WPM: " + (int)result.NetWPM, null, Console.CursorTop + 1);
            }
            else
            {
                await InputOutput.WritePretty("Net WPM: Less than 0 \n (Wow, how did you manage that?)", null, Console.CursorTop + 1);
            }
            await Task.Delay(1000);
        }

        /// <summary>
        /// Prompts the player for restart
        /// </summary>
        /// <returns>returns true if the player pressed the restart button</returns>
        public static async Task<bool> PromptRestart()
        {
            await InputOutput.WritePretty("\n\n\nPress Space to go again...", null, Console.CursorTop + 1);

            while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Spacebar)
            {
                await Task.Delay(10);
            }
            return true;
        }

        public enum WinMessages : uint
        {
            /// <summary>
            /// An application sends the WM_SETICON message to associate a new large or small icon with a window. 
            /// The system displays the large icon in the ALT+TAB dialog box, and the small icon in the window caption. 
            /// </summary>
            SETICON = 0x0080,
        }
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);


        /// <summary>
        /// Sets the application window icon
        /// </summary>
        private static void SetWindowIcon(System.Drawing.Icon icon)
        {
            IntPtr mwHandle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            IntPtr result01 = SendMessage(mwHandle, (int)WinMessages.SETICON, 0, icon.Handle);
            IntPtr result02 = SendMessage(mwHandle, (int)WinMessages.SETICON, 1, icon.Handle);
        }
    }
}