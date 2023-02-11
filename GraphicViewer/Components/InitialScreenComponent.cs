using Microsoft.Xna.Framework;

namespace GraphicViewer.Components
{
    internal class InitialScreenComponent : SKComponent<GraphicViewerGame>
    {
        SpectrumFont _font;

        public InitialScreenComponent(GraphicViewerGame game) : base(game)
        {
            _font = new SpectrumFont(game, "font", 16, 7);
            _font.Paper = SpectrumColor.Black;
            _font.Ink = SpectrumColor.White;
        }

        public override void Draw(GameTime gameTime)
        {
            //                                   01234567890123456789012345678901
            _font.Ink = SpectrumColor.White;
            _font.DrawString(Game.Buffer, 8, 8, "   ZX SPECTRUM GRAPHIC VIEWER", gameTime);
            _font.DrawString(Game.Buffer, 8, 16, "   --------------------------", gameTime);

            _font.DrawString(Game.Buffer, 8, 32, "A handy tool to extract sprites,", gameTime);
            _font.DrawString(Game.Buffer, 8, 40, "fonts, and SCREEN$ from .z80", gameTime);
            _font.DrawString(Game.Buffer, 8, 48, "files.", gameTime);

            _font.Ink = SpectrumColor.Yellow;
            _font.DrawString(Game.Buffer, 8, 64, "Press the &/ key to show the", gameTime);
            _font.DrawString(Game.Buffer, 8, 72, "menu. Pressing &/&Q quits.", gameTime);

            _font.Ink = SpectrumColor.White;
            _font.DrawString(Game.Buffer, 8, 88, "Full source on GitHub at:", gameTime);
            _font.Ink = SpectrumColor.Cyan;
            _font.DrawString(Game.Buffer, 8, 96, "https://github.com/codehoose", gameTime);

            base.Draw(gameTime);
        }
    }
}
