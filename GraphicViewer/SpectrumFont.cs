using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace GraphicViewer
{
    internal class SpectrumFont : SpriteSheet
    {
        private Texture2D _paper;

        public Color Ink { get; set; } = SpectrumColor.Black;

        public Color Paper { get; set; } = SpectrumColor.White;

        public SpectrumFont(Game game, string spriteSheetName, int columns, int rows) 
            : base(game, spriteSheetName, columns, rows)
        {
            _paper = new Texture2D(game.GraphicsDevice, 1, 1);
            _paper.SetData(new Color[] { Color.White });
        }

        public void DrawString(SpriteBatch spriteBatch, int x, int y, string message, GameTime gameTime, bool ampersandsAreSpecial = true)
        {
            int length = message.Where(ch => ch != '&').Count() * 8;
            int count = 0;

            for (int i = 0; i < message.Length; i++)
            {
                Color ink = Ink;
                Color paper = Paper;

                if (message[i] == '&' && ampersandsAreSpecial)
                {
                    ink = Paper;
                    paper = Ink;
                    i++;
                }

                int index = message[i] - ' ';
                spriteBatch.Draw(_paper, new Rectangle(count * 8, y, 8, 8), null, paper, 0, Vector2.Zero, SpriteEffects.None, 0.2f);
                Draw(spriteBatch, new Vector2(count * 8, y), index, gameTime, ink);
                    
                count++;
            }           
        }        
    }
}
