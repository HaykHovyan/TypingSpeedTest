using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TypeSpeedTest
{
    public static class InputOutput
    {
        public async static Task Write(string output, int cursorPositionX = 0, int cursorPositionY = 0)
        {

            int outputLength = output.Length;
            //set cursor position in the middle
            if (cursorPositionX == 0)
            {
                if (outputLength < Console.BufferWidth)
                {
                    cursorPositionX = (Console.BufferWidth - outputLength) / 2;
                }
                else
                {
                    cursorPositionX = 1;
                }
            }
            if (cursorPositionY == 0)
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
                    await Write(lastString, cursorPositionX, cursorPositionY++);
                    outputDivided.RemoveAt(outputDivided.Count - 1);
                }
            }
            else
            {
                Console.SetCursorPosition(cursorPositionX, cursorPositionY);

                for (int i = 0; i < output.Length; i++)
                {
                    if (Console.CursorLeft == Console.WindowWidth - 1)
                    {
                        cursorPositionY++;
                        Console.SetCursorPosition(cursorPositionX, cursorPositionY);
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