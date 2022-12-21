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
        SpriteFont shopText;
        Rectangle atkBuyRect;
        Texture2D sawBlade;
        Rectangle bladeBuyRect;
        double castleHealth;
        int coins;
        int bladeCount;
        int atkDamage;
        int wave;
        List<BasicEnemy> basicEnemys;
        List<Blade> bladesList;
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
            wave = 0;
            atkDamage = 1;
            bladeCount = 0;
            playButtonRect = new Rectangle(200, 530, 300, 150);
            castleRect = new Rectangle(275, 250, 150, 150);
            shopButton = new Rectangle(620, 5, 75, 75);
            atkBuyRect = new Rectangle(125, 25, 50, 50);
            bladeBuyRect = new Rectangle(200, 25, 50, 50);
            base.Initialize();
            ranGen = new Random();
            basicEnemys = new List<BasicEnemy>();
            bladesList = new List<Blade>();
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
            shopText = Content.Load<SpriteFont>("shopText");
            badGuyTextureDown = Content.Load<Texture2D>("basicEnemyDown");
            badGuyTextureUp = Content.Load<Texture2D>("basicEnemyUp");
            badGuyTextureLeft = Content.Load<Texture2D>("basicEnemyLeft");
            badGuyTextureRight = Content.Load<Texture2D>("basicEnemyRight");
            heartIcon = Content.Load<Texture2D>("heartIcon");
            coinIcon = Content.Load<Texture2D>("coinIcon");
            shopIcon = Content.Load<Texture2D>("shopIcon");
            swordIcon = Content.Load<Texture2D>("swordIcon");
            shopTint = Content.Load<Texture2D>("shopTint");
            sawBlade = Content.Load<Texture2D>("sawBlade");
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
                if (basicEnemys.Count == 0)
                {
                    for (int i = 0; i < 10; i++)
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

                bool test = false;

                for (int i = 0; i < basicEnemys.Count; i++)
                {
                    castleHealth = basicEnemys[i].Move(_graphics, castleRect, castleHealth);
                    foreach (Blade blade in bladesList)
                    {
                        if (!test && blade.BoundRect.Intersects(basicEnemys[i].BoundRect))
                        {
                            test = true;
                            basicEnemys[i].Damage(1);
                            if (basicEnemys[i].Health <= 0)
                            {
                                basicEnemys.RemoveAt(i);
                                coins += ranGen.Next(1, 4);
                                i--;
                            }
                        }
                    }
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
                foreach (Blade blade in bladesList)
                {
                    blade.Move(_graphics);
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
                    if (atkBuyRect.Contains(mouseState.X, mouseState.Y))
                    {
                        int cost = atkDamage * 10;
                        if (coins >= cost)
                        {
                            atkDamage++;
                            coins -= cost;
                        }
                    }
                    if (bladeBuyRect.Contains(mouseState.X, mouseState.Y))
                    {
                        int cost = (bladeCount * 100) + 100;
                        if (coins >= cost)
                        {
                            bladeCount++;
                            bladesList.Add(new Blade(new Rectangle(ranGen.Next(100, 600), ranGen.Next(100, 600), 30, 30), new Vector2(-2, 2), sawBlade));
                            coins -= cost;
                        }
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
                _spriteBatch.Draw(sawBlade, new Rectangle(0, 94, 22, 22), Color.White);
                _spriteBatch.DrawString(castleHealthText, $"{bladeCount}", new Vector2(30, 90), Color.LightGray);
                foreach (BasicEnemy enemyB in basicEnemys)
                {
                    enemyB.Draw(_spriteBatch, badGuyHealth);
                }
                foreach (Blade blade in bladesList)
                {
                    blade.Draw(_spriteBatch);
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
                _spriteBatch.Draw(sawBlade, new Rectangle(0, 94, 22, 22), Color.White);
                _spriteBatch.DrawString(castleHealthText, $"{bladeCount}", new Vector2(30, 90), Color.LightGray);
                foreach (BasicEnemy enemyB in basicEnemys)
                {
                    enemyB.Draw(_spriteBatch, badGuyHealth);
                }
                foreach (Blade blade in bladesList)
                {
                    blade.Draw(_spriteBatch);
                }
                _spriteBatch.Draw(shopTint, new Rectangle(100, 0, 520, 100), Color.White);

                _spriteBatch.Draw(swordIcon, atkBuyRect, Color.White);
                _spriteBatch.DrawString(shopText, $"{atkDamage} -> {atkDamage + 1}", new Vector2(125, 0), Color.LightGray);
                if (coins >= atkDamage * 10)
                    _spriteBatch.DrawString(shopText, $"${atkDamage * 10}", new Vector2(125, 76), Color.Green);
                else
                    _spriteBatch.DrawString(shopText, $"${atkDamage * 10}", new Vector2(125, 76), Color.Red);

                _spriteBatch.Draw(sawBlade, bladeBuyRect, Color.White);
                _spriteBatch.DrawString(shopText, $"{bladeCount} -> {bladeCount + 1}", new Vector2(200, 0), Color.LightGray);
                if (coins >= (bladeCount * 100) + 100)
                    _spriteBatch.DrawString(shopText, $"${(bladeCount * 100) + 100}", new Vector2(200, 76), Color.Green);
                else
                    _spriteBatch.DrawString(shopText, $"${(bladeCount * 100) + 100}", new Vector2(200, 76), Color.Red);
            }

            else if (screen == Screen.EndScreen)
            {

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}