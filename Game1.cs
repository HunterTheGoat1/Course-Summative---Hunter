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
        Texture2D badGuyTextureUp;
        Texture2D badGuyTextureDown;
        Texture2D badGuyTextureLeft;
        Texture2D badGuyTextureRight;
        Texture2D coinIcon;
        Texture2D heartIcon;
        Texture2D shopIcon;
        Texture2D shopTint;
        Texture2D swordIcon;
        Rectangle shopButton;
        double castleHealth;
        int coins;
        int atkDamage;
        List<BasicEnemy> basicEnemys;
        Random ranGen;
        Screen screen;
        enum Screen
        {
            MainMenu,
            GameScreen,
            ShopScreen,
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
            coins = 0;
            atkDamage = 1;
            playButtonRect = new Rectangle(200, 530, 300, 150);
            castleRect = new Rectangle(275, 250, 150, 150);
            shopButton = new Rectangle(620, 5, 75, 75);
            base.Initialize();
            ranGen = new Random();
            basicEnemys = new List<BasicEnemy>();

            for (int i = 0; i < 25; i++)
            {
                Texture2D basicTexture = badGuyTextureDown;
                int x = 0;
                int y = 0;
                int spawnSide = ranGen.Next(1, 5);
                if (spawnSide == 1)
                {
                    x = ranGen.Next(0, 650);
                    y = ranGen.Next(-300, 50);
                    basicTexture = badGuyTextureDown;
                }
                if (spawnSide == 2)
                {
                    x = ranGen.Next(0, 650);
                    y = ranGen.Next(600, 1000);
                    basicTexture = badGuyTextureUp;
                }
                if (spawnSide == 3)
                {
                    x = ranGen.Next(-300, 50);
                    y = ranGen.Next(0, 650);
                    basicTexture = badGuyTextureRight;
                }
                if (spawnSide == 4)
                {
                    x = ranGen.Next(600, 1000);
                    y = ranGen.Next(0, 650);
                    basicTexture = badGuyTextureLeft;
                }
                basicEnemys.Add(new BasicEnemy(new Rectangle(x, y, 50, 50), basicTexture, 2));
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
            badGuyTextureDown = Content.Load<Texture2D>("basicEnemyDown");
            badGuyTextureUp = Content.Load<Texture2D>("basicEnemyUp");
            badGuyTextureLeft = Content.Load<Texture2D>("basicEnemyLeft");
            badGuyTextureRight = Content.Load<Texture2D>("basicEnemyRight");
            heartIcon = Content.Load<Texture2D>("heartIcon");
            coinIcon = Content.Load<Texture2D>("coinIcon");
            shopIcon = Content.Load<Texture2D>("shopIcon");
            swordIcon = Content.Load<Texture2D>("swordIcon");
            shopTint = Content.Load<Texture2D>("shopTint");
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

            else if (screen == Screen.GameScreen)
            {
                bool test = false;
                for (int i = 0; i < basicEnemys.Count; i++)
                {
                    castleHealth = basicEnemys[i].Move(_graphics, castleRect, castleHealth);
                    // Detects a click on enemies, applies damage
                    if (!test && mouseState.LeftButton == ButtonState.Pressed && preMouseState.LeftButton == ButtonState.Released)
                    {
                        if (basicEnemys[i].BoundRect.Contains(mouseState.X, mouseState.Y))
                        {
                            test = true;
                            basicEnemys[i].Damage(atkDamage);
                            if (basicEnemys[i].Health <= 0)
                            {
                                basicEnemys.RemoveAt(i);
                                coins += ranGen.Next(1, 4);
                                i--;
                            }
                        }
                    }
                }
                if (mouseState.LeftButton == ButtonState.Pressed && preMouseState.LeftButton == ButtonState.Released)
                {
                    if (shopButton.Contains(mouseState.X, mouseState.Y))
                    {
                        screen = Screen.ShopScreen;
                    }
                }
            }
            else if (screen == Screen.ShopScreen)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && preMouseState.LeftButton == ButtonState.Released)
                {
                    if (shopButton.Contains(mouseState.X, mouseState.Y))
                    {
                        screen = Screen.GameScreen;
                    }
                }
            }

            else if (screen == Screen.EndScreen)
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

            else if (screen == Screen.GameScreen)
            {
                _spriteBatch.Draw(gameBackground, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
                _spriteBatch.Draw(mainCastle, castleRect, Color.White);
                _spriteBatch.Draw(heartIcon, new Rectangle(0, 0, 25, 25), Color.White);
                _spriteBatch.DrawString(castleHealthText, $"{System.Math.Round(castleHealth, 2)}", new Vector2(30, 0), Color.Red);
                _spriteBatch.Draw(coinIcon, new Rectangle(0, 32, 25, 25), Color.White);
                _spriteBatch.DrawString(castleHealthText, $"{coins}", new Vector2(30, 30), Color.Gold);
                _spriteBatch.Draw(shopIcon, shopButton, Color.White);
                _spriteBatch.Draw(swordIcon, new Rectangle(0, 64, 22, 22), Color.White);
                _spriteBatch.DrawString(castleHealthText, $"{atkDamage}", new Vector2(30, 60), Color.LightGray);
                foreach (BasicEnemy enemyB in basicEnemys)
                {
                    enemyB.Draw(_spriteBatch, badGuyHealth);
                }
            }

            else if (screen == Screen.ShopScreen)
            {
                _spriteBatch.Draw(gameBackground, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
                _spriteBatch.Draw(mainCastle, castleRect, Color.White);
                _spriteBatch.Draw(heartIcon, new Rectangle(0, 0, 25, 25), Color.White);
                _spriteBatch.DrawString(castleHealthText, $"{System.Math.Round(castleHealth, 2)}", new Vector2(30, 0), Color.Red);
                _spriteBatch.Draw(coinIcon, new Rectangle(0, 32, 25, 25), Color.White);
                _spriteBatch.DrawString(castleHealthText, $"{coins}", new Vector2(30, 30), Color.Gold);
                _spriteBatch.Draw(shopIcon, shopButton, Color.White);
                _spriteBatch.Draw(swordIcon, new Rectangle(0, 64, 22, 22), Color.White);
                _spriteBatch.DrawString(castleHealthText, $"{atkDamage}", new Vector2(30, 60), Color.LightGray);
                foreach (BasicEnemy enemyB in basicEnemys)
                {
                    enemyB.Draw(_spriteBatch, badGuyHealth);
                }
                _spriteBatch.Draw(shopTint, new Rectangle(100, 0, 520, 100), Color.White);
            }

            else if (screen == Screen.EndScreen)
            {

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}