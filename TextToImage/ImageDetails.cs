using System;
using System.Collections.Generic;
using System.Drawing;

namespace TextToImage
{
    public struct ImageText
    {
        public readonly String Text;
        public readonly Font Font;

        public ImageText(String text, Font font=null)
        {
            Text = text;

            if (font != null)
                Font = font;
            else
                Font = new Font(FontFamily.GenericSansSerif, (Single)72.0, FontStyle.Regular);
        }
    }

    public struct ImageDetails
    {
        public readonly String Path;
        public readonly Int32 Width;
        public readonly Color TextColor;
        public readonly Color BackgroundColor;
        public readonly List<ImageText> TextPieces;

        public ImageDetails(String path, Color textColor, Color backgroundColor, List<ImageText> textPieces, Int32 width = 1920)
        {
            Path = path;
            Width = width;
            TextColor = textColor;
            BackgroundColor = backgroundColor;
            TextPieces = textPieces;
        }
    }
}
