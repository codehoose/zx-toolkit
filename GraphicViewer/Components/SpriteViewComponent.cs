using MacGraphicViewer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace GraphicViewer.Components
{
    internal class SpriteViewComponent : SKComponent<GraphicViewerGame>
    {
        Texture2D _spriteView;
        Texture2D _infoView;
        Texture2D _selectionBox;
        int _offset;
        private MouseState _state;
        private bool _pressed;

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
            _spriteView = new Texture2D(game.GraphicsDevice, 256, 192);
            _spriteView.Fill(SpectrumColor.Black);
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


                if (_selectionBox != null)
                {
                    _selectionBox.Dispose();
                    _selectionBox = null;
                }

                // Limitation: can only have square selection boxes
                _selectionBox = new Texture2D(Game.GraphicsDevice, SpriteWidth, SpriteWidth);
                _selectionBox.Outline(Color.Red, new Color(0, 0, 0, 0));
            }

            _state = Mouse.GetState();
            if (!_pressed && _state.LeftButton == ButtonState.Pressed)
            {
                _pressed = true;
            }
            else if (_pressed && _state.LeftButton == ButtonState.Released)
            {
                _pressed = false;
                SaveAtCursor();
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
            
            if (SpriteWidth > _spriteView.Width)
                SpriteWidth = _spriteView.Width;

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
                            Color[] colors = Helpers.ByteToColor(bytes[c]);

                            int pixelIndex = (y * _spriteView.Width) + actualX;
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


        public override void Draw(GameTime gameTime)
        {
            Game.Buffer.Draw(_spriteView, Vector2.Zero, Color.White);
            //Game.Buffer.Draw(_infoView, new Rectangle(0, 160, 256, 4 * 8), Color.Black);
            
            if (_selectionBox != null)
            {
                // This screen is four times smaller than the game window. So the mouse x- and y- pos have
                // to be transformed into Spectrum screen space (256x192)
                Vector2 cursorPos = new Vector2(_state.X/4 - SpriteWidth / 2, _state.Y/4 - SpriteWidth / 2);
                Game.Buffer.Draw(_selectionBox, cursorPos, Color.White);
            }

            base.Draw(gameTime);
        }


        private void SaveAtCursor()
        {
            int x = _state.X / 4 - SpriteWidth / 2;
            int y = _state.Y / 4 - SpriteWidth / 2;

            if (x < 0 || x > 256 - SpriteWidth || y < 0 || y > 192 - SpriteWidth)
                return;

            Texture2D tex = _spriteView.GetSubTexture(Game.GraphicsDevice, new Rectangle(x, y, SpriteWidth, SpriteWidth));

            string fileName =   Path.GetRandomFileName() + ".png";
            using(Stream s = File.OpenWrite(fileName))
            {
                tex.SaveAsPng(s, SpriteWidth, SpriteWidth);
            }
        }
    }
}
