using GraphicViewer.Components;
using GraphicViewer.Components.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace GraphicViewer
{
    public class GraphicViewerGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _backBuffer;
        private Z80FileLoader _loader;
        private Z80File _file;

        private MenuSystem _menuSystem;
        private ScreenManager _screenManager;
        private InitialScreenComponent _initialScreen;

        public Z80File File => _file;

        public int MemoryOffset { get; set; } = 0;

        public bool Quit { get; set; }

        public SpriteBatch Buffer => _spriteBatch;

        internal ScreenManager ScreenManager => _screenManager;

        internal MenuSystem Menus => _menuSystem;

        private void LoadFile(string filename)
        {
            // Change the base directory here...
            string folder = @"C:\users\sloan\downloads";
            string file = Path.Combine(folder, filename);
            _file = _loader.Load(file);

            // Remove the menus
            _menuSystem.CloseAll();

            // Add the screen
            ShowHex();
        }

        internal void ShowHex()
        {
            _screenManager.Activate(() => new HexDumpComponent(this));
            _screenManager.GetScreen<HexDumpComponent>().ForceRefresh = true;
            _menuSystem.AddMenu(new HexMenuComponent(this));
        }

        internal void ShowScreen()
        {
            _screenManager.Activate(() => new ZXSpectrumScreenComponent(this));
            _screenManager.GetScreen<ZXSpectrumScreenComponent>().ForceRefresh = true;
        }

        internal void ShowSpriteView()
        {
            _screenManager.Activate(() => new SpriteViewComponent(this));
            _screenManager.GetScreen<SpriteViewComponent>().ForceRefresh = true;
            _menuSystem.AddMenu(new SpriteViewMenuComponent(this));
        }

        internal void LoadFile()
        {
            _menuSystem.AddMenu(new PlayerInputMenuComponent(this, (s) => LoadFile(s)));
        }

        public GraphicViewerGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.PreferredBackBufferWidth = 1024;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _backBuffer = new RenderTarget2D(GraphicsDevice, 256, 192);

            _loader = new Z80FileLoader();

            _menuSystem = new MenuSystem(this);
            Components.Add(_menuSystem);

            _screenManager = new ScreenManager(this);
            _initialScreen = new InitialScreenComponent(this);
            _screenManager.AddScreen(_initialScreen);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Quit)
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_backBuffer);
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            base.Draw(gameTime);
            _spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_backBuffer, new Rectangle(0, 0, 1024, 768), Color.White);
            _spriteBatch.End();
        }
    }
}