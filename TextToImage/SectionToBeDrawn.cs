using System;
using System.Collections.Generic;
using System.Text;

namespace TextToImage
{
    public class SectionToBeDrawn
    {
        public List<TextToBeDrawn> SectionParts { get; set; }

        public SectionToBeDrawn()
        {
            SectionParts = new List<TextToBeDrawn>();
        }
    }
}
