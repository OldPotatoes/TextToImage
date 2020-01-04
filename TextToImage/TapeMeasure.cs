using System;
using System.Drawing;

namespace TextToImage
{
    public sealed class TapeMeasure
    {
        private const Boolean DEBUG = true;
        private IGraphics _graphicsWrapper;

        public TapeMeasure() : this(GraphicsWrapper.Instance)
        {
        }

        public TapeMeasure(IGraphics graphicsWrapper)
        {
            _graphicsWrapper = graphicsWrapper;
        }

        ~TapeMeasure()
        {
            _graphicsWrapper.Cleanup();
        }

        public SectionToBeDrawn DefineTextToBeDrawn(SizeF pageSize, SizeF startingLocation, String text, Font font)
        {
            var section = new SectionToBeDrawn();
            if (text.Length == 0)
                return section;

            Int32 whiteSpaceIndex = 0;
            String textRemaining = text;
            SizeF cursorLocation = startingLocation;

            while (textRemaining.Length > 0)
            {
                Boolean reachedEndOfLine = false;
                Int32 previousWhiteSpaceIndex = 0;
                SizeF blockSize = new SizeF();
                String blockText = String.Empty;

                text = text.Remove(0, whiteSpaceIndex).TrimStart();
                whiteSpaceIndex = 0;

                while ((textRemaining.Length > 0) && (!reachedEndOfLine))
                {
                    whiteSpaceIndex = FindNextWordEnding(text, whiteSpaceIndex + 1);
                    blockText = text.Substring(0, whiteSpaceIndex);

                    textRemaining = text.Substring(whiteSpaceIndex);
                    blockSize = MeasureText(blockText, font);

                    // Line passes right margin. Remove last word added
                    if (cursorLocation.Width + blockSize.Width > pageSize.Width)
                    {
                        reachedEndOfLine = true;
                        whiteSpaceIndex = previousWhiteSpaceIndex;
                        blockText = text.Substring(0, whiteSpaceIndex);
                        textRemaining = text.Substring(whiteSpaceIndex);

                        if (blockText.Length > 0)
                            blockSize = MeasureText(blockText, font);
                        else
                        {
                            blockSize.Width = pageSize.Width - cursorLocation.Width + 1;
                        }
                    }
                    // If text contains pilcrow:
                    else if (blockText.Contains('¶'))
                    {
                        // Treat it as white space
                        whiteSpaceIndex = text.IndexOf('¶');
                        blockText = text.Substring(0, whiteSpaceIndex);

                        // Remove it from the text string
                        text = text.Remove(whiteSpaceIndex, 1);

                        reachedEndOfLine = true;
                        blockSize.Width = pageSize.Width;
                    }

                    previousWhiteSpaceIndex = whiteSpaceIndex;
                }

                var block = new TextToBeDrawn
                {
                    Text = blockText,
                    Font = font,
                    LeftEdge = cursorLocation.Width,
                    TopEdge = cursorLocation.Height,
                    Width = blockSize.Width,
                    Height = blockSize.Height
                };

                section.SectionParts.Add(block);

                cursorLocation.Width += blockSize.Width;

                if (cursorLocation.Width >= pageSize.Width)
                {
                    cursorLocation.Width = 0;
                    cursorLocation.Height += blockSize.Height;
                }
            }

            return section;
        }

        public SizeF MeasureText(String text, Font font)
        {
            // If string is empty, give it some height.
            if (text == String.Empty)
                text = "I";

            SizeF textSize;
            using (Image dummyImage = new Bitmap(1, 1))
            {
                _graphicsWrapper.CreateDrawingSurface(dummyImage, Color.White, Color.Black);
                textSize = _graphicsWrapper.MeasureString(text, font);
            }

            if (DEBUG)
                Console.WriteLine($"TapeMeasure: Text '{text}' is {textSize} pixels long.");

            return textSize;
        }

        public static Int32 FindNextWordEnding(String text, Int32 currentWordEnding = 0)
        {
            Int32 count = currentWordEnding;
            while (count < text.Length)
            {
                if (Char.IsWhiteSpace(text, count))
                    break;

                count++;
            }

            if (DEBUG)
                Console.WriteLine($"TapeMeasure: In text '{text}' the next word ending after {currentWordEnding} is {count} characters in.");

            return count;
        }
    }
}
