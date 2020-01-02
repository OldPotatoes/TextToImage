using System;
using System.Drawing;

namespace TextToImage
{
    public class TextToBeDrawn
    {
        public readonly Font DefaultFont = new Font(FontFamily.GenericSerif, 72.0f, FontStyle.Regular);

        public String Text { get; set; }
        public Font Font { get; set; }
        public Single LeftEdge { get; set; }
        public Single TopEdge { get; set; }
        public Single Width { get; set; }
        public Single Height { get; set; }

        public TextToBeDrawn()
        {
            Text = String.Empty;
            Font = DefaultFont;
        }
    }
}