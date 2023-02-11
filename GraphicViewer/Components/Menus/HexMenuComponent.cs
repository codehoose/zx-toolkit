using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicViewer.Components.Menus
{
    internal class HexMenuComponent : Menu
    {
        public HexMenuComponent(GraphicViewerGame game)
            : base(game, "&/ Back &P&g&U&p and &P&g&D&o&w&n to Scroll")
        {
        }

        public override void Process(Keys key)
        {
            if (key == Keys.PageUp)
            {
                Game.ScreenManager.GetScreen<HexDumpComponent>().PageUp();

            }
            else if (key == Keys.PageDown)
            {
                Game.ScreenManager.GetScreen<HexDumpComponent>().PageDown();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
