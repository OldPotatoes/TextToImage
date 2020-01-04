using System;
using System.Collections.Generic;
using System.Drawing;
using TextToImage;

namespace ExampleTextToImage
{
    class Program
    {
        static void Main()
        {
            String path = @"C:\temp\image.png";
            //String textTitle1 = "MOBY-DICK;";
            //String textTitle2 = "or, THE WHALE.";
            //String textAuthor = "By Herman Melville";
            //String textChapter = "CHAPTER 1. Loomings.";
            //String text1 = "Call me Ishmael. Some years ago—never mind how long precisely—having little or no money in my purse, and nothing particular to interest me on shore, I thought I would sail about a little and see the watery part of the world. It is a way I have of driving off the spleen and regulating the circulation. Whenever I find myself growing grim about the mouth; whenever it is a damp, drizzly November in my soul; whenever I find myself involuntarily pausing before coffin warehouses, and bringing up the rear of every funeral I meet; and especially whenever my hypos get such an upper hand of me, that it requires a strong moral principle to prevent me from deliberately stepping into the street, and methodically knocking people’s hats off—then, I account it high time to get to sea as soon as I can. This is my substitute for pistol and ball. With a philosophical flourish Cato throws himself upon his sword; I quietly take to the ship. There is nothing surprising in this. If they but knew it, almost all men in their degree, some time or other, cherish very nearly the same feelings towards the ocean with me.";
            //String text2 = "There now is your insular city of the Manhattoes, belted round by wharves as Indian isles by coral reefs—commerce surrounds it with her surf. Right and left, the streets take you waterward. Its extreme downtown is the battery, where that noble mole is washed by waves, and cooled by breezes, which a few hours previous were out of sight of land. Look at the crowds of water-gazers there.";

            String textCharacter = "Character: ";
            String textTitle = "Syrio Forel";
            String text1 = "“Hear me. The ships of Braavos sail as far as the winds blow, to lands strange and wonderful, and when they return their captains fetch queer animals to the Sealord’s menagerie. Such animals as you have never seen, striped horses, great spotted things with necks as long as stilts, hairy mouse-pigs as big as cows, stinging manticores, tigers that carry their cubs in a pouch, terrible walking lizards with scythes for claws. Syrio Forel has seen these things.";
            String textSpace = "";
            String text2 = "“On the day I am speaking of, the first sword was newly dead, and the Sealord sent for me. Many bravos had come to him, and as many had been sent away, none could say why. When I came into his presence, he was seated, and in his lap was a fat yellow cat. He told me that one of his captains had brought the beast to him, from an island beyond the sunrise. ‘Have you ever seen her like?’ he asked of me.";
            String text3 = "“And to him I said, ‘Each night in the alleys of Braavos I see a thousand like him,’ and the Sealord laughed, and that day I was named the first sword.”";
            String text4 = "Arya screwed up her face. “I don’t understand.”";
            String text5 = "Syrio clicked his teeth together. “The cat was an ordinary cat, no more. The others expected a fabulous beast, so that is what they saw. How large it was, they said. It was no larger than any other cat, only fat from indolence, for the Sealord fed it from his own table. What curious small ears, they said. Its ears had been chewed away in kitten fights. And it was plainly a tomcat, yet the Sealord said ‘her,’ and that is what the others saw. Are you hearing?”";
            String text6 = "Arya thought about it. “You saw what was there.”";
            String text7 = "“Just so. Opening your eyes is all that is needing. The heart lies and the head plays tricks with us, but the eyes see true. Look with your eyes. Hear with your ears. Taste with your mouth. Smell with your nose. Feel with your skin. Then comes the thinking, afterward, and in that way knowing the truth.”";

            Font fontBigBold = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
            Font fontNormalBold = new Font(FontFamily.GenericSansSerif, (Single)60.0, FontStyle.Bold);
            Font fontBig = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Regular);
            Font fontText = new Font(FontFamily.GenericSansSerif, (Single)48.0, FontStyle.Regular);

            var pen = new InkPen();
            pen.CreateImage(new ImageDetails(path,
                                             Color.AntiqueWhite,
                                             Color.Black,
                                             new List<List<ImageText>>() {
                                                 new List<ImageText> { new ImageText(textCharacter, fontBig), new ImageText(textTitle, fontBigBold) },
                                                 new List<ImageText> { new ImageText(textSpace, fontText) },
                                                 new List<ImageText> { new ImageText(text1, fontText) },
                                                 new List<ImageText> { new ImageText(textSpace, fontText) },
                                                 new List<ImageText> { new ImageText(text2, fontText) },
                                                 new List<ImageText> { new ImageText(textSpace, fontText) },
                                                 new List<ImageText> { new ImageText(text3, fontText) },
                                                 new List<ImageText> { new ImageText(textSpace, fontText) },
                                                 new List<ImageText> { new ImageText(text4, fontText) },
                                                 new List<ImageText> { new ImageText(textSpace, fontText) },
                                                 new List<ImageText> { new ImageText(text5, fontText) },
                                                 new List<ImageText> { new ImageText(textSpace, fontText) },
                                                 new List<ImageText> { new ImageText(text6, fontText) },
                                                 new List<ImageText> { new ImageText(textSpace, fontText) },
                                                 new List<ImageText> { new ImageText(text7, fontText) }
                                             }));
        }
    }
}
