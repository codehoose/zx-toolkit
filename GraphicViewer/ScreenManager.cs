using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphicViewer
{
    internal class ScreenManager : SKTickable<GraphicViewerGame>
    {
        private readonly List<GameComponent> _components = new List<GameComponent>();

        public ScreenManager(GraphicViewerGame game) : base(game)
        {
        }

        public void AddScreen(GameComponent component)
        {
            while (_components.Count > 0)
            {
                Game.Components.Remove(Pop());
            }

            Push(component);
            Game.Components.Add(component);
        }

        public void RemoveTop()
        {
            if (_components.Count > 0)
            {
                Game.Components.Remove(Pop());
            }

            if (_components.Count > 0)
            {
                Game.Components.Add(Peek());
            }
        }

        internal void Activate<T>(Func<T> create) where T : GameComponent
        {
            T instance = _components.FirstOrDefault(c => c is T) as T;
            if (instance != null)
            {
                _components.Remove(instance);
                Game.Components.Remove(instance);
                AddScreen(instance);
            }
            else
            {
                AddScreen(create());
            }
        }

        private GameComponent Pop()
        {
            if (_components.Count > 0)
            {
                GameComponent comp = _components[0];
                _components.RemoveAt(0);
                return comp;
            }

            return null;
        }

        internal T GetScreen<T>() where T : GameComponent
        {
            T instance = _components.FirstOrDefault(c => c is T) as T;
            return instance;
        }

        private void Push(GameComponent component)
        {
            _components.Add(component);
        }

        private GameComponent Peek()
        {
            if (_components.Count > 0) return _components[0];
            return null;
        }
    }
}
