using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GraphicViewer.Components
{
    internal class SpriteViewComponent : SKComponent<GraphicViewerGame>
    {
        Texture2D _spriteView;
        Texture2D _infoView;
        int _offset;

        public bool ForceRefresh { get; set; }

        public bool RowOrder { get; set; } = true;

        public int Columns { get; set; } = 1;

        public int SpriteWidth { get; set; } = 16;

        public int Offset
        {
            get => _offset;
            set => _offset = value;
        }

        public SpriteViewComponent(GraphicViewerGame game) : base(game)
        {
            _spriteView = new Texture2D(game.GraphicsDevice, 160, 160);
            _infoView = new Texture2D(game.GraphicsDevice, 1, 1);
            _infoView.SetData(new Color[] { Color.Black });
            Offset = 12804;
        }

        public override void Update(GameTime gameTime)
        {
            if (ForceRefresh)
            {
                if (_offset < 0)
                    _offset = 0;
                if (_offset > 65535 - 3200)
                {
                    _offset = 65535 - 3200;
                }

                ForceRefresh = false;
                ShowRowOrderFixed();
            }
        }

        private void ShowRowOrderFixed()
        {
            int offset = _offset;

            int x = 0;
            int y = 0;

            if (Columns < 0)
                Columns = 1;

            if (Columns > 10)
                Columns = 10;

            if (SpriteWidth < 8)
                SpriteWidth = 8;
            
            if (SpriteWidth > 160)
                SpriteWidth = 160;

            Color[] pixels = new Color[_spriteView.Width * _spriteView.Height];
            int numColumns = SpriteWidth / 8;
            for (int cc = 0; cc < Columns; cc++)
            {
                for (int yc = 0; yc < 16; yc++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        byte[] bytes = new byte[numColumns];
                        Array.Copy(Game.File.Memory, offset, bytes, 0, numColumns);
                        for (int c = 0; c < bytes.Length; c++)
                        {
                            int actualX = (x + c * 8) + cc * SpriteWidth;
                            Color[] colors = ByteToColor(bytes[c]);

                            int pixelIndex = (y * 160) + actualX;
                            Array.Copy(colors, 0, pixels, pixelIndex, colors.Length);
                        }
                        offset += 2;
                        y++;
                    }
                }
                y = 0;
            }

            _spriteView.SetData(pixels);
        }

        private void ShowSingleChar()
        {
            int offset = _offset;

            int x = 0;
            int y = 0;

            if (Columns < 0)
                Columns = 1;

            if (Columns > 16)
                Columns = 16;

            Color[] pixels = new Color[_spriteView.Width * _spriteView.Height];

            while (y < 16)
            {
                for (int i =0; i < Columns; i++)
                {
                    byte[] bytes = new byte[8];
                    Array.Copy(Game.File.Memory, offset, bytes, 0, 8);
                    
                    for (int row = 0; row < 8; row++)
                    {
                        Color[] colors = ByteToColor(bytes[row]);
                        int pixelIndex = (y * 8 + row) * _spriteView.Width + x;
                        Array.Copy(colors, 0, pixels, pixelIndex, 8);
                    }
                    offset += 8;
                    x += 8;
                }

                y++;
                x = 0;
            }


            _spriteView.SetData(pixels);
        }

        private void ShowRowOrder()
        {
            Color[] pixels = new Color[_spriteView.Width * _spriteView.Height];
            var data = Game.File.Memory;
            int offset = _offset;
            for (int j = 0; j < 100; j++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < 2; x++)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            int jy = j / 10;
                            int jx = j % 10;

                            Color[] bits = ByteToColor(data[offset++]);
                            int yy = jy * 2 + y;
                            int xx = jx * 2 + x;
                            Array.Copy(bits, 0, pixels, (160 * (i + (yy * 8))) + (xx * 8), bits.Length);
                        }
                    }
                }
            }

            _spriteView.SetData(pixels);
        }

        private void ShowColumnOrder()
        {
            Color[] pixels = new Color[_spriteView.Width * _spriteView.Height];
            var data = Game.File.Memory;
            int offset = _offset;
            for (int j = 0; j < 100; j++)
            {
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            int jy = j / 10;
                            int jx = j % 10;

                            Color[] bits = ByteToColor(data[offset++]);
                            int yy = jy * 2 + y;
                            int xx = jx * 2 + x;
                            Array.Copy(bits, 0, pixels, (160 * (i + (yy * 8))) + (xx * 8), bits.Length);
                        }
                    }
                }
            }

            _spriteView.SetData(pixels);
        }

        private Color[] ByteToColor(byte b)
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

        public override void Draw(GameTime gameTime)
        {
            Game.Buffer.Draw(_spriteView, Vector2.Zero, Color.White);
            Game.Buffer.Draw(_infoView, new Rectangle(0, 160, 256, 4 * 8), Color.Black);
            base.Draw(gameTime);
        }
    }
}
