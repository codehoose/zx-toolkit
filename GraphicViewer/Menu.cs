using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GraphicViewer
{
    internal class Menu : SKComponent<GraphicViewerGame>
    {
        SpectrumFont _font;
        string _menuShape;

        protected string MenuShape
        {
            get => _menuShape;
            set => _menuShape = value;
        }

        protected SpectrumFont Font => _font;

        public Menu(GraphicViewerGame game, string menuShape) : base(game)
        {
            _font = new SpectrumFont(game, "font", 16, 7);
            _font.Ink = SpectrumColor.Red;
            _font.Paper = SpectrumColor.Yellow;
            _menuShape = menuShape;
        }

        public virtual void Process(Keys key)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            _font.DrawString(Game.Buffer, 0, 192 - 8, _menuShape, gameTime);
            base.Draw(gameTime);
        }
    }
}
