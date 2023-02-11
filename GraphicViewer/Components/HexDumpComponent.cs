using Microsoft.Xna.Framework;

namespace GraphicViewer.Components
{
    internal class HexDumpComponent : SKComponent<GraphicViewerGame>
    {
        SpectrumFont _font;
        int _offset;

        public bool ForceRefresh { get; set; }

        public void PageDown()
        {
            _offset += (20 * 8);
            if (_offset > 65535)
                _offset = 65535 - (20 * 8);
            ForceRefresh = true;
        }

        public void PageUp()
        {
            _offset -= (20 * 8);
            if (_offset < 0)
                _offset = 0;
            ForceRefresh = true;
        }

        public HexDumpComponent(GraphicViewerGame game) : base(game)
        {
            _font = new SpectrumFont(game, "font", 16, 7);
            _font.Paper = SpectrumColor.Black;
            _font.Ink = SpectrumColor.White;
        }

        public override void Update(GameTime gameTime)
        {
            if (ForceRefresh)
            {
                ForceRefresh = false;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _font.Ink = SpectrumColor.White;
            var data = Game.File.Memory;

            int y = 0;
            for (int i = 0; i < 20 * 8; i += 8)
            {
                int ptr = _offset + i;
                int mem = ptr + 16384;

                _font.DrawString(Game.Buffer, 0, y, $"{mem:X0000}  {GetHex(data, ptr)}  {GetString(data, ptr)}", gameTime, false);
                y += 8;
            }
            base.Draw(gameTime);
        }

        private string GetHex(byte[] data, int offset)
        {
            string msg = "";
            for (int i = 0; i < 8; i++)
            {
                msg += string.Format("{0:X2}", data[offset + i]);
            }
            return msg;
        }

        private string GetString(byte[] data, int offset)
        {
            string msg = "";
            for(int i = 0; i < 8;i++)
            {
                int index = data[offset + i] - ' ';
                if (index <0 || index >=128)
                {
                    msg += "?";
                }
                else
                {
                    msg += (char)(data[offset + i]);
                }
            }

            return msg;
        }

    }
}
