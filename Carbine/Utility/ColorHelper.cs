// Decompiled with JetBrains decompiler
// Type: Carbine.Utility.ColorHelper
// Assembly: Carbine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9929100E-21E2-4663-A88C-1F977D6B46C4
// Assembly location: D:\OddityPrototypes\Carbine.dll

using SFML.Graphics;

namespace Carbine.Utility
{
    public static class ColorHelper
    {
        public static Color FromInt(int color) => ColorHelper.FromInt((uint)color);

        public static Color FromInt(uint color)
        {
            byte alpha = (byte)(color >> 24);
            return new Color((byte)(color >> 16), (byte)(color >> 8), (byte)color, alpha);
        }

        public static Color Blend(Color col1, Color col2, float amount)
        {
            float num = 1f - amount;
            return new Color((byte)(col1.R * (double)num + col2.R * (double)amount), (byte)(col1.G * (double)num + col2.G * (double)amount), (byte)(col1.B * (double)num + col2.B * (double)amount), byte.MaxValue);
        }

        public static Color BlendAlpha(Color col1, Color col2, float amount)
        {
            float num = 1f - amount;
            return new Color((byte)(col1.R * (double)num + col2.R * (double)amount), (byte)(col1.G * (double)num + col2.G * (double)amount), (byte)(col1.B * (double)num + col2.B * (double)amount), (byte)(col1.A * (double)num + col2.A * (double)amount));
        }

        public static Color Invert(this Color color) => new Color((byte)(byte.MaxValue - (uint)color.R), (byte)(byte.MaxValue - (uint)color.G), (byte)(byte.MaxValue - (uint)color.B), color.A);
    }
}
