namespace TypingSpeedTest
{
    public static class InputOutput
    {
        public static async Task WritePretty(string output, int? cursorPositionX = null, int? cursorPositionY = null)
        {

            int outputLength = output.Length;
            //set cursor position in the middle
            if (cursorPositionX == null)
            {
                if (outputLength < Console.BufferWidth)
                {
                    cursorPositionX = (Console.BufferWidth - outputLength) / 2;
                }
                else
                {
                    cursorPositionX = 0;
                }
            }
            if (cursorPositionY == null)
            {
                cursorPositionY = (Console.WindowHeight / 2);
            }

            if (output.Contains('\n'))
            {
                List<string> outputDivided = output.Split('\n').Reverse().ToList();
                while (outputDivided.Count != 0)
                {
                    string lastString = outputDivided.Last();
                    cursorPositionX = (Console.BufferWidth - lastString.Length) / 2;
                    await WritePretty(lastString, cursorPositionX, cursorPositionY++);
                    outputDivided.RemoveAt(outputDivided.Count - 1);
                }
            }
            else
            {
                Console.SetCursorPosition((int)cursorPositionX, (int)cursorPositionY);

                for (int i = 0; i < output.Length; i++)
                {
                    if (Console.CursorLeft == Console.WindowWidth - 1)
                    {
                        cursorPositionY++;
                        Console.SetCursorPosition((int)cursorPositionX, (int)cursorPositionY);
                    }

                    Console.Write(output[i]);
                    await Task.Delay(25);
                }
            }
        }

        public static char Read()
        {
            char input = default;
            if (Console.KeyAvailable)
            {
                input = Console.ReadKey(true).KeyChar;
            }
            return input;
        }
    }
}