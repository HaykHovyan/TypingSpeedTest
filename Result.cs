namespace TypingSpeedTest
{
    public class Result
    {
        public int CharactersTyped { get; set; }
        public float GrossWPM { get; set; }
        public float NetWPM { get; set; }
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