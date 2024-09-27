namespace TypingSpeedTest
{
    public class Result
    {
        public int CharactersTyped { get; set; }
        public int GrossWPM { get; set; }
        public int NetWPM { get; set; }
        public List<char> Mistakes { get; set; }

        public Result()
        {
            CharactersTyped = 0;
            GrossWPM = 0;
            NetWPM = 0;
            Mistakes = new List<char>();
        }
    }
}
