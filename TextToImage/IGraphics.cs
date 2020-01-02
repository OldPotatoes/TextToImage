using System;
using System.Drawing;

namespace TextToImage
{
    public interface IGraphics
    {
        Graphics CreateDrawingSurface(Image image, Color textColour, Color backgroundColour);
        void DrawString(String text, Font font, Single xUpperLeft, Single yUpperLeft);
        SizeF MeasureString(String text, Font font);
        void SaveDrawing();

        void Cleanup();
    }
}
