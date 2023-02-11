using GraphicViewer.Components.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphicViewer
{
    internal class MenuSystem : SKTickable<GraphicViewerGame>
    {
        private readonly Stack<Menu> _menus = new Stack<Menu>();
        private Keys[] _lastKeys = new Keys[] { };

        public MenuSystem(GraphicViewerGame game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            Keys[] keys = state.GetPressedKeys();

            List<Keys> process = new List<Keys>();
            for (int i = 0; i < _lastKeys.Length; i++)
            {
                if (!keys.Contains(_lastKeys[i]))
                {
                    process.Add(_lastKeys[i]);
                }
            }

            _lastKeys = keys;

            foreach (var key in process)
            {
                if (key == Keys.OemQuestion) // This is the initial show menu
                {
                    if (_menus.Count == 0)
                    {
                        AddMenu(new MainMenuComponent(Game));
                    }
                    else
                    {
                        CloseMenu();
                        AddMenu(new MainMenuComponent(Game));
                    }

                    //else
                    //{
                    //    CloseMenu();
                    //}
                }
                else if (_menus.Count > 0)
                {
                    _menus.Peek().Process(key);
                }
            }

            base.Update(gameTime);
        }

        public void AddMenu(Menu menu)
        {
            if (_menus.Count > 0)
            {
                Game.Components.Remove(_menus.Peek());
            }

            _menus.Push(menu);
            Game.Components.Add(_menus.Peek());
        }

        public void CloseAll()
        {
            while(_menus.Count >0)
            {
                Game.Components.Remove(_menus.Pop());
            }
        }

        public void CloseMenu()
        {
            if (_menus.Count > 0)
            {
                Game.Components.Remove(_menus.Peek());
            }
        }
    }
}
