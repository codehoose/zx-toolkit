using Microsoft.Xna.Framework;

namespace GraphicViewer
{
    internal class SKComponent<T> : DrawableGameComponent where T: Game
    {
        private T _instance;

        public new T Game => _instance;

        public SKComponent(T game): base(game)
        {
            _instance = game;
        }
    }

    internal class SKTickable<T> : GameComponent where T : Game
    {
        private T _instance;

        public new T Game => _instance;

        public SKTickable(T game) : base(game)
        {
            _instance = game;
        }
    }
}
