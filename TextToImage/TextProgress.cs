using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TextToImage
{
    public class TextProgress
    {
        public Single LineHeight { get; set; }
        public SizeF NextLocation { get; set; }
        public String TextWritten { get; set; }
        public String LineTextRemaining { get; set; }
    }
}
