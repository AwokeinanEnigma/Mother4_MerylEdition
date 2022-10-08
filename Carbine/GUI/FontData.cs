using SFML.Graphics;
using System;
using Carbine.Utility;

namespace Carbine.GUI
{
    public class FontData : IDisposable
    {
        private const uint W_CODE_POINT = 41;
        private bool disposed;
        private Font font;
        private int xComp;
        private int yComp;
        private int lineHeight;
        private int wHeight;
        private uint fontSize;
        private float alphaThreshold;

        public Font Font
        {
            get
            {
                return this.font;
            }
        }

        public int XCompensation
        {
            get
            {
                return this.xComp;
            }
        }

        public int YCompensation
        {
            get
            {
                return this.yComp;
            }
        }

        public int LineHeight
        {
            get
            {
                return this.lineHeight;
            }
        }

        public int WHeight
        {
            get
            {
                return this.wHeight;
            }
        }

        public uint Size
        {
            get
            {
                return this.fontSize;
            }
        }

        public float AlphaThreshold
        {
            get
            {
                return this.alphaThreshold;
            }
        }

        public FontData()
        {
            this.font = new Font(EmbeddedResources.GetStream("Carbine.Resources.openSansPX.ttf"));
            this.fontSize = 16U;
            this.wHeight = (int)this.font.GetGlyph(41U, this.fontSize, false).Bounds.Height;
            this.lineHeight = (int)((double)this.wHeight * 1.20000004768372);
            this.alphaThreshold = 0.0f;
        }

        public FontData(Font font, uint fontSize, int lineHeight, int xComp, int yComp)
        {
            this.font = font;
            this.fontSize = fontSize;
            this.lineHeight = lineHeight;
            this.xComp = xComp;
            this.yComp = yComp;
            this.wHeight = (int)this.font.GetGlyph(41U, this.fontSize, false).Bounds.Height;
           // Console.WriteLine($"wHeight = {wHeight}");
            this.alphaThreshold = 0.8f;
        }

        ~FontData()
        {
            this.Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
                this.font.Dispose();
            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }
    }
}
