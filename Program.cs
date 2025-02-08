namespace TypingSpeedTest
{
    class Program
    {
        /// <summary>
        /// Entry point for the program
        /// </summary>
        public static async Task Main()
        {
            GUI.Initialize();
            await GUI.PromptStart();

            Game game = new Game();
            await game.Start();

            bool gameFinished = false;
            while (!gameFinished)
            {
                gameFinished = await game.Update();
            }
        }
    }
}