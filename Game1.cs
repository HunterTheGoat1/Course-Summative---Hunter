using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

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
        List<BasicEnemy> basicEnemys;
        Random ranGen;
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
            ranGen = new Random();
            basicEnemys = new List<BasicEnemy>();

            for (int i = 0; i < 10; i++)
            {
                int x = 0;
                int y = 0;
                int spawnSide = ranGen.Next(1, 5);
                if(spawnSide == 1)
                {
                    x = ranGen.Next(0, 650);
                    y = ranGen.Next(-300, 50); ;
                }
                if(spawnSide == 2)
                {
                    x = ranGen.Next(0, 650);
                    y = ranGen.Next(600, 1000);
                }
                if(spawnSide == 3)
                {
                    x = ranGen.Next(-300, 50); ;
                    y = ranGen.Next(0, 650);
                }
                if(spawnSide == 4)
                {
                    x = ranGen.Next(600, 1000);
                    y = ranGen.Next(0, 650);
                }
                basicEnemys.Add(new BasicEnemy(new Rectangle(x, y, 50, 50), badGuyTexture, 5));
            }
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
                int i = 0;
                foreach (BasicEnemy enemyB in basicEnemys)
                {
                    if (mouseState.LeftButton == ButtonState.Pressed && preMouseState.LeftButton == ButtonState.Released)
                        if (enemyB.BoundRect.Contains(mouseState.X, mouseState.Y))
                            enemyB.Damage(1);
                    if (enemyB.Health <= 0)
                    {
                        basicEnemys.RemoveAt(i);
                    }
                    castleHealth = enemyB.Move(_graphics, castleRect, castleHealth);
                    i += 1;
                }
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
                foreach (BasicEnemy enemyB in basicEnemys)
                {
                    enemyB.Draw(_spriteBatch, badGuyHealth);
                }
            }

            if (screen == Screen.EndScreen)
            {

            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}