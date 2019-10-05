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
            String textTitle1 = "MOBY-DICK;";
            String textTitle2 = "or, THE WHALE.";
            String textAuthor = "By Herman Melville";
            String textChapter = "CHAPTER 1. Loomings.";
            String text1 = "Call me Ishmael. Some years ago—never mind how long precisely—having little or no money in my purse, and nothing particular to interest me on shore, I thought I would sail about a little and see the watery part of the world. It is a way I have of driving off the spleen and regulating the circulation. Whenever I find myself growing grim about the mouth; whenever it is a damp, drizzly November in my soul; whenever I find myself involuntarily pausing before coffin warehouses, and bringing up the rear of every funeral I meet; and especially whenever my hypos get such an upper hand of me, that it requires a strong moral principle to prevent me from deliberately stepping into the street, and methodically knocking people’s hats off—then, I account it high time to get to sea as soon as I can. This is my substitute for pistol and ball. With a philosophical flourish Cato throws himself upon his sword; I quietly take to the ship. There is nothing surprising in this. If they but knew it, almost all men in their degree, some time or other, cherish very nearly the same feelings towards the ocean with me.";
            String text2 = "There now is your insular city of the Manhattoes, belted round by wharves as Indian isles by coral reefs—commerce surrounds it with her surf. Right and left, the streets take you waterward. Its extreme downtown is the battery, where that noble mole is washed by waves, and cooled by breezes, which a few hours previous were out of sight of land. Look at the crowds of water-gazers there.";

            Font fontTitle = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
            Font fontAuthor = new Font(FontFamily.GenericSerif, (Single)72.0, FontStyle.Bold);
            Font fontChapter = new Font(FontFamily.GenericSansSerif, (Single)60.0, FontStyle.Bold);
            Font fontText = new Font(FontFamily.GenericSansSerif, (Single)24.0, FontStyle.Regular);

            var pen = new InkPen();
            pen.CreateImage(new ImageDetails(path,
                                             Color.AntiqueWhite,
                                             Color.Black,
                                             new List<ImageText>() {
                                                 new ImageText(textTitle1, fontTitle),
                                                 new ImageText(textTitle2, fontTitle),
                                                 new ImageText(textAuthor, fontAuthor),
                                                 new ImageText(textChapter, fontChapter),
                                                 new ImageText(text1, fontText),
                                                 new ImageText(text2, fontText)
                                             }));
        }
    }
}
