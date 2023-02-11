using Microsoft.Xna.Framework.Input;

namespace GraphicViewer.Components.Menus
{
    internal class SpriteViewMenuComponent : Menu
    {
        public SpriteViewMenuComponent(GraphicViewerGame game) 
            : base(game, "PgUp/Down L/R R=toggle order".PadRight(37, ' '))
        {
        }

        public override void Process(Keys key)
        {
            SpriteViewComponent svc = Game.ScreenManager.GetScreen<SpriteViewComponent>();

            switch(key)
            {
                case Keys.PageDown:
                    svc.Offset += 160;
                    svc.ForceRefresh = true;
                    break;
                case Keys.PageUp:
                    svc.Offset -= 160;
                    svc.ForceRefresh = true;
                    break;
                case Keys.Right:
                    svc.Offset += 1;
                    svc.ForceRefresh = true;
                    break;
                case Keys.Left:
                    svc.Offset -= 1;
                    svc.ForceRefresh = true;
                    break;
                case Keys.R:
                    svc.RowOrder = !svc.RowOrder;
                    svc.ForceRefresh = true;
                    break;
                case Keys.Up:
                    svc.Columns += 1;
                    svc.ForceRefresh = true;
                    break;
                case Keys.Down:
                    svc.Columns -= 1;
                    svc.ForceRefresh = true;
                    break;
                case Keys.OemPlus:
                    svc.SpriteWidth += 8;
                    svc.ForceRefresh = true;
                    break;
                case Keys.OemMinus:
                    svc.SpriteWidth -= 8;
                    svc.ForceRefresh = true;
                    break;
            }
        }
    }
}
