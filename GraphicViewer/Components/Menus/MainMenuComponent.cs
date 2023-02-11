using Microsoft.Xna.Framework.Input;

namespace GraphicViewer.Components.Menus
{
    internal class MainMenuComponent : Menu
    {
        public MainMenuComponent(GraphicViewerGame game)
            : base(game, "&Load &View &Save &Quit             ")
        {
        }

        public override void Process(Keys key)
        {
            switch(key)
            {
                case Keys.V:
                    Game.Menus.AddMenu(new ViewMenuComponent(Game));
                    break;
                case Keys.Q:
                    Game.Quit = true;
                    break;
                case Keys.L:
                    Game.LoadFile();
                    break;
            }
        }
    }
}
