using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GraphicViewer.Components
{
    internal class ZXSpectrumScreenComponent : SKComponent<GraphicViewerGame>
    {
        private int _memoryOffset;
        private Texture2D _screen;

        public bool ForceRefresh { get; set; }

        public ZXSpectrumScreenComponent(GraphicViewerGame game) : base(game)
        {
            _memoryOffset = -1;
            _screen = new Texture2D(game.GraphicsDevice, 256, 192);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_memoryOffset != Game.MemoryOffset || ForceRefresh)
            {
                _memoryOffset = Game.MemoryOffset;
                RedrawScreen();
                ForceRefresh = false;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Game.Buffer.Draw(_screen, Vector2.Zero, Color.White);
            base.Draw(gameTime);
        }

        private void RedrawScreen()
        {
            Color[] colors = new Color[256 * 192];

            for (int i = 0; i < 6144; i++)
            {
                // From here: http://www.breakintoprogram.co.uk/hardware/computers/zx-spectrum/screen-memory-layout
                int offset = 16384 + i;
                int x = (offset & 0x1f) * 8;

                int h = (offset & 0xff00) >> 8; // 8 most significant bits
                int y = h & 7; // bottom 3 bits

                y = y | (offset & 0xe0) >> 2;
                y = y | (h & 24) << 3;

                int index = y * 256 + x;
                int bit = 7;

                byte mem = Game.File.Memory[i + _memoryOffset];

                var colours = GetAttr(Game.File.Memory, x / 8, y / 8);

                while (bit >= 0)
                {
                    int mask = 1 << bit;
                    if ((mem & mask) == mask)
                    {
                        colors[index++] = colours.Item1;
                    }
                    else
                    {
                        colors[index++] = colours.Item2;
                    }
                    bit--;
                }
            }

            _screen.SetData(colors);
        }

        private Tuple<Color, Color> GetAttr(byte[] memory, int x, int y)
        {
            int offset = _memoryOffset + 6144 + (y * 32) + x;
            int ink = memory[offset] & 7;
            int paper = (memory[offset] >> 3) & 7;
            Color inkColor = SpectrumColor.Indexed[ink];
            Color paperColor = SpectrumColor.Indexed[paper];
            return Tuple.Create(inkColor, paperColor);
        }
    }
}
