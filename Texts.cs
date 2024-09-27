using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeSpeedTest
{
    public static class Texts
    {
        //private string text;
        //public string Text { get { return text; } }

        static readonly string text1 = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
            "Nullam vehicula laoreet turpis vitae placerat. " +
            "Quisque suscipit efficitur ante, a porta sapien vestibulum sed. " +
            "Suspendisse vel rhoncus sem, non volutpat enim. " + 
            "Mauris sollicitudin ligula sed consequat porta. " +
            "Phasellus sagittis dictum magna, sit amet imperdiet felis vulputate ut. " +
            "Vivamus fringilla posuere ante. Suspendisse eget eros euismod, " +
            "egestas mauris in, sodales sem. " +
            "Quisque vel augue dolor. " +
            "Duis sit amet felis ut nunc eleifend pulvinar. " +
            "Mauris elit arcu, bibendum at nisl eget, congue aliquet neque. " +
            "Nunc posuere convallis purus non consequat. " +
            "Vestibulum blandit commodo lectus. Morbi eget diam tortor. " +
            "Phasellus non vestibulum tellus. In hac habitasse platea dictumst. " +
            "Nam ut erat non ante volutpat elementum. ";
        static readonly string text2 = "Lord of the Flies by British author William Golding " +
            "was first published in 1954. " +
            "Set against the backdrop of a deserted island during an unspecified wartime, " +
            "the novel tells the gripping story of a group of boys stranded after their plane crashes. " +
            "Initially, the boys attempt to establish a society with rules and order, " +
            "choosing a boy named Ralph as their leader. However, as the days pass, " +
            "the fragile social order disintegrates, revealing the darker side of human nature. " +
            "The descent into chaos is marked by the emergence of a primal and violent force " +
            "embodied by a character named Jack, " +
            "leading to the loss of civility and the breakdown of morality. Golding’s work is a powerful exploration " +
            "of the inherent capacity for savagery within human beings when societal structures are removed.";
        static readonly string text3 = "Imagine a world forever altered by the brief visit of an inscrutable alien civilization, " +
            "where mysterious Zones left behind are littered with baffling and perilous artifacts. " +
            "In \"Roadside Picnic\" by Arkady Strugatsky, " +
            "humanity grapples with profound and haunting questions about our place in the universe, " +
            "the nature of intelligence, and the ethical quandaries of curiosity. " +
            "Through the eyes of Redrick \"Red\" Schuhart, a seasoned and haunted stalker who clandestinely ventures into the Zone, " +
            "the novel captures an eerie and gripping atmosphere where each step could mean unimaginable fortune or instant doom. " +
            "This masterful blend of science fiction, philosophy, " +
            "and existential dread invites readers to explore not just the hauntingly alien landscape, " +
            "but the very soul of humanity itself.";
        static List<string> TextList;

        static Texts()
        {
            TextList = new List<string>
            {
                text1,
                text2,
                text3
            };
        }

        public static string GetText()
        {
            var random = new Random();
            int index = random.Next(0, TextList.Count - 1);
            return TextList[index];
        }
    }
}
