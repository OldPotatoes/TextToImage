using System;
using System.Collections.Generic;
using System.Drawing;
using TextToImage;

namespace ExampleTextToImage
{
    class Program
    {
        static void Main(string[] args)
        {
            String path = @"C:\temp\image.png";
            //String text1 = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
            String textTitle = "Hoster Tully";
            String textSubheading = "Chapters Survived: 24";
            String textSubheading2 = "Some other sub-heading";
            String textParagraph = "They laid Lord Hoster in a slender wooden boat, clad in shining silver armor, plate-and-mail. His cloak was spread beneath him, rippling blue and red. His surcoat was divided blue-and-red as well. A trout, scaled in silver and bronze, crowned the crest of the greathelm they placed beside his head. On his chest they placed a painted wooden sword, his fingers curled about its hilt. Mail gauntlets hid his wasted hands, and made him look almost strong again. His massive oak-and-iron shield was set by his left side, his hunting horn to his right. The rest of the boat was filled with driftwood and kindling and scraps of parchment, and stones to make it heavy in the water. His banner flew from the prow, the leaping trout of Riverrun.";

            Font fontTitle = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
            Font fontSubheading = new Font(FontFamily.GenericSansSerif, (Single)86.0, FontStyle.Bold);
            Font fontSubheading2 = new Font(FontFamily.GenericSerif, (Single)72.0, FontStyle.Bold);
            Font fontParagraph = new Font(FontFamily.GenericSansSerif, (Single)72.0, FontStyle.Regular);

            //var tti = new TextToImage.TextToImage();
            //tti.CreateImage(new ImageDetails(path, 
            //                                 Color.AntiqueWhite,
            //                                 Color.Black,
            //                                 new List<ImageText>() {
            //                                     new ImageText(textTitle, fontTitle),
            //                                     new ImageText(textSubheading, fontSubheading),
            //                                     new ImageText(textSubheading2, fontSubheading2),
            //                                     new ImageText(textParagraph, fontParagraph)
            //                                 })
            //);

            var pen = new InkPen();
            pen.CreateImage(new ImageDetails(path,
                                             Color.AntiqueWhite,
                                             Color.Black,
                                             new List<ImageText>() {
                                                 new ImageText(textTitle, fontTitle),
                                                 new ImageText(textSubheading, fontSubheading),
                                                 new ImageText(textSubheading2, fontSubheading2),
                                                 new ImageText(textParagraph, fontParagraph)
                                             }));
        }
    }
}
