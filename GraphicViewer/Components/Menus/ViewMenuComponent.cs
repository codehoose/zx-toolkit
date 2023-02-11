using Microsoft.Xna.Framework.Input;

namespace GraphicViewer.Components.Menus
{
    internal class ViewMenuComponent : Menu
    {
        public ViewMenuComponent(GraphicViewerGame game) 
            : base(game, "&Hex &Screen S&prites &Char".PadRight(37, ' '))
        {
        }

        public override void Process(Keys key)
        {
            switch(key)
            {
                case Keys.H:
                    Game.ShowHex();
                    break;
                case Keys.S:
                    Game.ShowScreen();
                    break;
                case Keys.P:
                    Game.ShowSpriteView();
                    break;
            }
        }
    }
}
