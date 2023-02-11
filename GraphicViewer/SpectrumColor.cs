using Microsoft.Xna.Framework;

namespace GraphicViewer
{
    internal class SpectrumColor
    {
        public static Color Black = Color.Black;
        public static Color Blue = new Color(0x00, 0x00, 0xd8, 0xff);
        public static Color Red = new Color(0xd8, 0x00, 0x00, 0xff);
        public static Color Magenta = new Color(0xd8, 0x00, 0xd8, 0xff);
        public static Color Green = new Color(0x00, 0xd8, 0x00, 0xff);
        public static Color Cyan = new Color(0x00, 0xd8, 0xd8, 0xff);
        public static Color Yellow = new Color(0xd8, 0xd8, 0x00, 0xff);
        public static Color White = new Color(0xd8, 0xd8, 0xd8, 0xff);

        public static Color BlackBright = Color.Black;
        public static Color BlueBright = new Color(0x00, 0x00, 0xd8, 0xff);
        public static Color RedBright = new Color(0xd8, 0x00, 0x00, 0xff);
        public static Color MagentaBright = new Color(0xd8, 0x00, 0xd8, 0xff);
        public static Color GreenBright = new Color(0x00, 0xd8, 0x00, 0xff);
        public static Color CyanBright = new Color(0x00, 0xd8, 0xd8, 0xff);
        public static Color YellowBright = new Color(0xd8, 0xd8, 0x00, 0xff);
        public static Color WhiteBright = new Color(0xd8, 0xd8, 0xd8, 0xff);

        public static Color[] Indexed = new Color[]
        {
            Black, Blue, Red, Magenta, Green, Cyan, Yellow, White
        };

        public static Color[] IndexedBright = new Color[]
        {
            BlackBright, BlueBright, RedBright, MagentaBright, GreenBright, CyanBright, YellowBright, WhiteBright
        };
    }
}
