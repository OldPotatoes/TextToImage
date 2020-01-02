using System;
using System.Drawing;

namespace TextToImage
{
    public class GraphicsWrapper : IGraphics, IDisposable
    {
        private static readonly Lazy<GraphicsWrapper> lazy = new Lazy<GraphicsWrapper>(() => new GraphicsWrapper());
        public static GraphicsWrapper Instance { get { return lazy.Value; } }

        private GraphicsWrapper()
        {
        }

        private Brush _solidBrush;
        private Graphics _drawingSurface;
        public Graphics DrawingSurface { get; }

        public Graphics CreateDrawingSurface(Image image, Color textColour, Color backgroundColour)
        {
            _solidBrush = new SolidBrush(textColour);
            _drawingSurface = Graphics.FromImage(image);
            _drawingSurface.Clear(backgroundColour);
            _drawingSurface.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            return _drawingSurface;
        }

        public SizeF MeasureString(String text, Font font)
        {
            return _drawingSurface.MeasureString(text, font);
        }

        public void DrawString(String text, Font font, Single xUpperLeft, Single yUpperLeft)
        {
            _drawingSurface.DrawString(text, font, _solidBrush, xUpperLeft, yUpperLeft);
        }

        public void SaveDrawing()
        {
            //_drawingSurface.Save();
        }

        public void Cleanup()
        {
            Dispose();
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _solidBrush.Dispose();
                    _drawingSurface.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
