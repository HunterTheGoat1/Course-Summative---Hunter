using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Course_Summative___Hunter
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D gameBackground;
        Texture2D mainCastle;
        Texture2D menuBackground;
        Texture2D playButton;
        Rectangle playButtonRect;
        MouseState mouseState;
        MouseState preMouseState; 
        Rectangle castleRect;
        SpriteFont castleHealthText;
        SpriteFont badGuyHealth;
        Texture2D badGuyTexture;
        double castleHealth;
        BasicEnemy test;
        Screen screen;
        enum Screen
        {
            MainMenu,
            GameScreen,
            EndScreen
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 700;
            _graphics.PreferredBackBufferHeight = 700;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            this.Window.Title = "Castle Hold Up";
            screen = Screen.MainMenu;
            castleHealth = 100;
            playButtonRect = new Rectangle(200, 530, 300, 150);
            castleRect = new Rectangle(275, 250, 150, 150);
            base.Initialize();
            test = new BasicEnemy(new Rectangle(0, 0, 50, 50), badGuyTexture, 5);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            gameBackground = Content.Load<Texture2D>("gameBackground");
            mainCastle = Content.Load<Texture2D>("castle");
            menuBackground = Content.Load<Texture2D>("menuBackground");
            playButton = Content.Load<Texture2D>("playButton");
            castleHealthText = Content.Load<SpriteFont>("healthText");
            badGuyHealth = Content.Load<SpriteFont>("badGuyHealth");
            badGuyTexture = Content.Load<Texture2D>("basicEnemy");
        }

        protected override void Update(GameTime gameTime)
        {
            preMouseState = mouseState;
            mouseState = Mouse.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (screen == Screen.MainMenu)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && preMouseState.LeftButton == ButtonState.Released)
                    if (playButtonRect.Contains(mouseState.X, mouseState.Y))
                        screen = Screen.GameScreen;
            }

            if (screen == Screen.GameScreen)
            {
                castleHealth = test.Move(_graphics, castleRect, castleHealth);
            }

            if (screen == Screen.EndScreen)
            {

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (screen == Screen.MainMenu)
            {
                _spriteBatch.Draw(menuBackground, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
                _spriteBatch.Draw(playButton, playButtonRect, Color.White);
            }

            if (screen == Screen.GameScreen)
            {
                _spriteBatch.Draw(gameBackground, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
                _spriteBatch.Draw(mainCastle, castleRect, Color.White);
                _spriteBatch.DrawString(castleHealthText, $"Castle: {System.Math.Round(castleHealth, 2)}", new Vector2(_graphics.PreferredBackBufferWidth/2 - 80, 0), Color.Red);
                test.Draw(_spriteBatch, badGuyHealth);
            }

            if (screen == Screen.EndScreen)
            {

            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}