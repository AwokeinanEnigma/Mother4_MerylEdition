using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using Carbine.Utility;

namespace Carbine.Graphics
{
    public class IndexedTexture : ICarbineTexture, IDisposable
    {
        [SuppressUnmanagedCodeSecurity]
        [DllImport("csfml-graphics-2", CallingConvention = CallingConvention.Cdecl)]
        private unsafe static extern void sfTexture_updateFromPixels(IntPtr texture, byte* pixels, uint width, uint height, uint x, uint y);

        public Texture Image
        {
            get
            {
                return this.imageTex;
            }
        }

        public Texture Palette
        {
            get
            {
                return this.paletteTex;
            }
        }

        public uint CurrentPalette
        {
            get
            {
                return this.currentPal;
            }
            set
            {
                this.currentPal = Math.Min(this.totalPals, value);
            }
        }

        public float CurrentPaletteFloat
        {
            get
            {
                return this.currentPal / this.totalPals;
            }
        }

        public uint PaletteCount
        {
            get
            {
                return this.totalPals;
            }
        }

        public uint PaletteSize
        {
            get
            {
                return this.palSize;
            }
        }

        public unsafe IndexedTexture(uint width, int[][] palettes, byte[] image, Dictionary<int, SpriteDefinition> definitions, SpriteDefinition defaultDefinition)
        {
            this.totalPals = (uint)palettes.Length;
            this.palSize = (uint)palettes[0].Length;
            uint num = (uint)(image.Length / (int)width);
            Color[] array = new Color[this.palSize * this.totalPals];
            for (uint num2 = 0U; num2 < this.totalPals; num2 += 1U)
            {
                uint num3 = 0U;
                while (num3 < (ulong)palettes[(int)((UIntPtr)num2)].Length)
                {
                    array[(int)((UIntPtr)(num2 * this.palSize + num3))] = ColorHelper.FromInt(palettes[(int)((UIntPtr)num2)][(int)((UIntPtr)num3)]);
                    num3 += 1U;
                }
            }
            Color[] array2 = new Color[width * num];
            uint num4 = 0U;
            while (num4 < (ulong)image.Length)
            {
                array2[(int)((UIntPtr)num4)].A = byte.MaxValue;
                array2[(int)((UIntPtr)num4)].R = image[(int)((UIntPtr)num4)];
                array2[(int)((UIntPtr)num4)].G = image[(int)((UIntPtr)num4)];
                array2[(int)((UIntPtr)num4)].B = image[(int)((UIntPtr)num4)];
                num4 += 1U;
            }
            this.paletteTex = new Texture(this.palSize, this.totalPals);
            this.imageTex = new Texture(width, num);
            fixed (Color* ptr = array)
            {
                byte* pixels = (byte*)ptr;
                IndexedTexture.sfTexture_updateFromPixels(this.paletteTex.CPointer, pixels, this.palSize, this.totalPals, 0U, 0U);
            }
            fixed (Color* ptr2 = array2)
            {
                byte* pixels2 = (byte*)ptr2;
                IndexedTexture.sfTexture_updateFromPixels(this.imageTex.CPointer, pixels2, width, num, 0U, 0U);
            }
            this.definitions = definitions;
            this.defaultDefinition = defaultDefinition;
        }

        ~IndexedTexture()
        {
            this.Dispose(false);
        }

        public SpriteDefinition GetSpriteDefinition(string name)
        {
            
            int hashCode = name.GetHashCode();
            return this.GetSpriteDefinition(hashCode);
        }

        public SpriteDefinition GetSpriteDefinition(int hash)
        {
            if (!this.definitions.TryGetValue(hash, out SpriteDefinition result))
            {
                result = null;
            }
            return result;
        }

        public ICollection<SpriteDefinition> GetSpriteDefinitions()
        {
            return this.definitions.Values;
        }

        public SpriteDefinition GetDefaultSpriteDefinition()
        {
            return this.defaultDefinition;
        }

        public FullColorTexture ToFullColorTexture()
        {
            uint x = this.imageTex.Size.X;
            uint y = this.imageTex.Size.Y;
            Image image = new Image(x, y);
            Image image2 = this.imageTex.CopyToImage();
            Image image3 = this.paletteTex.CopyToImage();
            for (uint num = 0U; num < y; num += 1U)
            {
                for (uint num2 = 0U; num2 < x; num2 += 1U)
                {
                    uint x2 = (uint)(image2.GetPixel(num2, num).R / 255.0 * this.palSize);
                    Color pixel = image3.GetPixel(x2, this.currentPal);
                    image.SetPixel(num2, num, pixel);
                }
            }
            image.SaveToFile("img.png");
            image2.SaveToFile("indImg.png");
            image3.SaveToFile("palImg.png");
            return new FullColorTexture(image);
        }

        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                this.imageTex.Dispose();
                this.paletteTex.Dispose();
            }
            this.disposed = true;
        }

        private SpriteDefinition defaultDefinition;

        private Dictionary<int, SpriteDefinition> definitions;

        private Texture paletteTex;

        private Texture imageTex;

        private uint currentPal;

        private uint totalPals;

        private uint palSize;

        private bool disposed;
    }
}
