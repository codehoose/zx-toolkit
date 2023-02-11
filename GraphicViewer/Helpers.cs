using GraphicViewer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MacGraphicViewer
{
    internal static class Helpers
    {
        public static Color[] ByteToColor(byte b)
        {
            Color[] colors = new Color[8];
            int index = 0;
            int bit = 7;
            while (bit >= 0)
            {
                int mask = 1 << bit;
                if ((b & mask) == mask)
                {
                    colors[index++] = SpectrumColor.White;
                }
                else
                {
                    colors[index++] = SpectrumColor.Black;
                }
                bit--;
            }
            return colors;
        }

        public static void Fill(this Texture2D texture, Color color)
        {
            Color[] colors = new Color[texture.Width * texture.Height];
            Array.Fill(colors, color);
            texture.SetData(colors);
        }

        public static void Outline(this Texture2D texture,Color outlineColor, Color insideColor)
        {
            Color[] colors = new Color[texture.Width * texture.Height];
            int index = 0;
            for (int y = 0; y < texture.Height; y++)
            {
                for(int x= 0; x < texture.Width; x++)
                {
                    Color color = x == 0 || y == 0 || x == texture.Width - 1 || y == texture.Height - 1 ? outlineColor : insideColor;
                    colors[index++] = color;
                }
            }

            texture.SetData(colors);
        }

        public static Texture2D GetSubTexture(this Texture2D texture, GraphicsDevice device, Rectangle rectangle)
        {
            Texture2D tex = new Texture2D(device, rectangle.Width , rectangle.Height);
            Color[] colors = new Color[rectangle.Width * rectangle.Height];

            Color[] source = new Color[texture.Width * texture.Height];
            texture.GetData(source);

            for (int x = 0; x < rectangle.Width; x++)
            {
                for (int y = 0; y < rectangle.Height; y++)
                {
                    colors[x + y * rectangle.Width] = source[x + rectangle.X + (y + rectangle.Y) * texture.Width];
                }
            }

            tex.SetData(colors);
            return tex;
        }
    }
}
