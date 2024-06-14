using System;
using System.Timers;
using System.Xml.XPath;

namespace TypeSpeedTest
{
    class Program
    {
        public static async Task Main()
        {
            using (var game = new Game())
            {
                GUI.Initialize();

                await Task.Run(async () =>
                {
                    await game.Start();
                    while (true)
                    {
                        await game.Update();
                    }
                });

                var ts = new TimeSpan(1, 0, 0, 0);
                Thread.Sleep(ts);
            }
        }
    }
}