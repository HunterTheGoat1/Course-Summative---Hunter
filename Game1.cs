//Game By Hunter Wilson, ICS4U-01
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

        //Textures
        Texture2D gameTitle;
        Texture2D gameBackground;
        Texture2D howToPlayTexture;
        Texture2D castleTexture;
        Texture2D menuBackground;
        Texture2D playButton;
        Texture2D badGuyTextureUp;
        Texture2D badGuyTextureDown;
        Texture2D badGuyTextureLeft;
        Texture2D badGuyTextureRight;
        Texture2D reinforcedEnemyUp;
        Texture2D reinforcedEnemyDown;
        Texture2D reinforcedEnemyLeft;
        Texture2D reinforcedEnemyRight;
        Texture2D ramEnemyUp;
        Texture2D ramEnemyDown;
        Texture2D ramEnemyLeft;
        Texture2D ramEnemyRight;
        Texture2D defenderUp;
        Texture2D defenderDown;
        Texture2D defenderLeft;
        Texture2D defenderRight;
        Texture2D coinIcon;
        Texture2D heartIcon;
        Texture2D shopIcon;
        Texture2D shopTint;
        Texture2D swordIcon;
        Texture2D defenderIcon;
        Texture2D sawBladeTexture;
        Texture2D bulletTexture;

        //Rectangles
        Rectangle castleRect;
        Rectangle playButtonRect;
        Rectangle shopButtonRect;
        Rectangle atkBuyRect;
        Rectangle bladeBuyRect;
        Rectangle howToPlayRect;
        Rectangle defenderBuyRect;

        //SpriteFonts
        SpriteFont castleHealthText;
        SpriteFont badGuyHealthText;
        SpriteFont shopText;

        //Global Numbers
        double castleHealth;
        int coins;
        int bladeCount;
        int atkDamage;
        int wave;
        int defenderCount;
        int bulletSpawnDelay;

        //Lists
        List<BasicEnemy> basicEnemys;
        List<Blade> bladesList;
        List<ReinforcedEnemy> reinforcedEnemyList;
        List<RamEnemy> ramEnemyList;
        List<Bullet> bulletList;

        //MouseStates
        MouseState mouseState;
        MouseState preMouseState;

        //Random Gen
        Random ranGen;

        //Screens
        Screen screen;
        enum Screen
        {
            MainMenu,
            HowToPlay,
            GameScreen,
            ShopScreen,
            EndScreen
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //Sets The Window Size
            _graphics.PreferredBackBufferWidth = 700;
            _graphics.PreferredBackBufferHeight = 700;

            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            //Sets Window Title
            this.Window.Title = "Castle Hold Up";

            //Sets The Screen
            screen = Screen.MainMenu;

            //Number Setup
            castleHealth = 100;
            coins = 0;
            wave = 0;
            atkDamage = 1;
            bladeCount = 0;
            defenderCount = 0;
            bulletSpawnDelay = 0;

            //Rectangle Setup
            playButtonRect = new Rectangle(200, 530, 300, 150);
            castleRect = new Rectangle(275, 250, 150, 150);
            shopButtonRect = new Rectangle(620, 5, 75, 75);
            atkBuyRect = new Rectangle(150, 25, 50, 50);
            bladeBuyRect = new Rectangle(250, 25, 50, 50);
            defenderBuyRect = new Rectangle(350, 25, 50, 50);
            howToPlayRect = new Rectangle(10, 10, 150, 75);

            base.Initialize();

            //Sets Up Random Gen
            ranGen = new Random();

            //Sets Up Lists
            basicEnemys = new List<BasicEnemy>();
            bladesList = new List<Blade>();
            reinforcedEnemyList = new List<ReinforcedEnemy>();
            ramEnemyList = new List<RamEnemy>();
            bulletList = new List<Bullet>();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Loads Textures
            gameTitle = Content.Load<Texture2D>("gameTitle");
            gameBackground = Content.Load<Texture2D>("gameBackground");
            castleTexture = Content.Load<Texture2D>("castle");
            menuBackground = Content.Load<Texture2D>("menuBackground");
            playButton = Content.Load<Texture2D>("playButton");
            badGuyTextureDown = Content.Load<Texture2D>("basicEnemyDown");
            badGuyTextureUp = Content.Load<Texture2D>("basicEnemyUp");
            badGuyTextureLeft = Content.Load<Texture2D>("basicEnemyLeft");
            badGuyTextureRight = Content.Load<Texture2D>("basicEnemyRight");
            reinforcedEnemyUp = Content.Load<Texture2D>("reinforcedEnemyUp");
            reinforcedEnemyDown = Content.Load<Texture2D>("reinforcedEnemyDown");
            reinforcedEnemyLeft = Content.Load<Texture2D>("reinforcedEnemyLeft");
            reinforcedEnemyRight = Content.Load<Texture2D>("reinforcedEnemyRight");
            ramEnemyUp = Content.Load<Texture2D>("ramEnemyUp");
            ramEnemyDown = Content.Load<Texture2D>("ramEnemyDown");
            ramEnemyLeft = Content.Load<Texture2D>("ramEnemyLeft");
            ramEnemyRight = Content.Load<Texture2D>("ramEnemyRight");
            heartIcon = Content.Load<Texture2D>("heartIcon");
            coinIcon = Content.Load<Texture2D>("coinIcon");
            shopIcon = Content.Load<Texture2D>("shopIcon");
            swordIcon = Content.Load<Texture2D>("swordIcon");
            defenderIcon = Content.Load<Texture2D>("defenderIcon");
            shopTint = Content.Load<Texture2D>("shopTint");
            sawBladeTexture = Content.Load<Texture2D>("sawBlade");
            howToPlayTexture = Content.Load<Texture2D>("HowToPlay");
            defenderUp = Content.Load<Texture2D>("defenderUp");
            defenderDown = Content.Load<Texture2D>("defenderDown");
            defenderLeft = Content.Load<Texture2D>("defenderLeft");
            defenderRight = Content.Load<Texture2D>("defenderRight");
            bulletTexture = Content.Load<Texture2D>("bullet");

            //Loads SpriteFonts
            castleHealthText = Content.Load<SpriteFont>("healthText");
            badGuyHealthText = Content.Load<SpriteFont>("badGuyHealth");
            shopText = Content.Load<SpriteFont>("shopText");

        }

        protected override void Update(GameTime gameTime)
        {
            //Sets Mouse States
            preMouseState = mouseState;
            mouseState = Mouse.GetState();

            //Allows Escape To Quit Game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //The Main Menu Screen
            if (screen == Screen.MainMenu)
            {
                //Checks If User Clicks Play, Then Changes The Screen To The Game Screen If They Pressed It
                if (mouseState.LeftButton == ButtonState.Pressed && preMouseState.LeftButton == ButtonState.Released)
                    if (playButtonRect.Contains(mouseState.X, mouseState.Y))
                        screen = Screen.GameScreen;
                //Checks If User Clicks How To Play, Then Changes The Screen To The How To Play Screen If They Pressed It
                if (mouseState.LeftButton == ButtonState.Pressed && preMouseState.LeftButton == ButtonState.Released)
                    if (howToPlayRect.Contains(mouseState.X, mouseState.Y))
                        screen = Screen.HowToPlay;
            }

            //The How To Play Screen
            if (screen == Screen.MainMenu)
            {
                //Checks If User Clicks Back To Main Menu
                if (mouseState.LeftButton == ButtonState.Pressed && preMouseState.LeftButton == ButtonState.Released)
                    if (playButtonRect.Contains(mouseState.X, mouseState.Y))
                        screen = Screen.MainMenu;
            }

            //The Game Screen
            else if (screen == Screen.GameScreen)
            {
                //Checks If The Wave Is Over
                if (basicEnemys.Count == 0 && reinforcedEnemyList.Count == 0 && ramEnemyList.Count == 0)
                {
                    //Clears bullets
                    bulletList.Clear();

                    //Adds Wave Count And Sets Up The Five Wave Count, Five Wave Count Goes Up By 1 Every 5 Waves
                    wave++;
                    double fiveWaveCount = wave / 5;
                    fiveWaveCount = Convert.ToInt32(System.Math.Round(fiveWaveCount));

                    //Makes The New List For The Current Wave, Uses Five Wave Count And Math To Increase Wave Difficulty
                    for (int i = 0; i < (fiveWaveCount + 10); i++)
                    {
                        //Makes Random Spawn Side Then Apply's The Right Texture To The Bad Guy
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
                        //Adds Bad Guy To The Wave List
                        basicEnemys.Add(new BasicEnemy(new Rectangle(x, y, 50, 50), basicTexture, Convert.ToInt32(System.Math.Round(fiveWaveCount + 1))));
                    }
                    int reinforcedSpawnAmmount = ranGen.Next(0, Convert.ToInt32(System.Math.Round(fiveWaveCount)));
                    //Checks if reinforced enemy's should spawn, only after round 20
                    if (reinforcedSpawnAmmount > 3)
                    {
                        reinforcedSpawnAmmount -= 3;
                        //Makes The New List For The Current Wave, Uses Five Wave Count And Math To Increase Wave Difficulty
                        for (int i = 0; i < reinforcedSpawnAmmount; i++)
                        {
                            //Makes Random Spawn Side Then Apply's The Right Texture To The Reinforced Bad Guy
                            Texture2D basicTexture = reinforcedEnemyDown;
                            int x = 0;
                            int y = 0;
                            int spawnSide = ranGen.Next(1, 5);
                            if (spawnSide == 1)
                            {
                                x = ranGen.Next(0, 650);
                                y = ranGen.Next(-300, 50);
                                basicTexture = reinforcedEnemyDown;
                            }
                            if (spawnSide == 2)
                            {
                                x = ranGen.Next(0, 650);
                                y = ranGen.Next(600, 1000);
                                basicTexture = reinforcedEnemyUp;
                            }
                            if (spawnSide == 3)
                            {
                                x = ranGen.Next(-300, 50);
                                y = ranGen.Next(0, 650);
                                basicTexture = reinforcedEnemyRight;
                            }
                            if (spawnSide == 4)
                            {
                                x = ranGen.Next(600, 1000);
                                y = ranGen.Next(0, 650);
                                basicTexture = reinforcedEnemyLeft;
                            }
                            //Adds Bad Guy To The Wave List
                            reinforcedEnemyList.Add(new ReinforcedEnemy(new Rectangle(x, y, 60, 60), basicTexture, Convert.ToInt32(System.Math.Round((fiveWaveCount + 1) * 2))));
                        }
                    }
                    int ramSpawnAmmount = ranGen.Next(0, Convert.ToInt32(System.Math.Round(fiveWaveCount)));
                    //Checks if ram enemy's should spawn, only after round 15
                    if (ramSpawnAmmount > 2)
                    {
                        ramSpawnAmmount -= 2;
                        //Makes The New List For The Current Wave, Uses Five Wave Count And Math To Increase Wave Difficulty
                        for (int i = 0; i < ramSpawnAmmount; i++)
                        {
                            //Makes Random Spawn Side Then Apply's The Right Texture To The Reinforced Bad Guy
                            Texture2D basicTexture = ramEnemyDown;
                            int x = 0;
                            int y = 0;
                            int spawnSide = ranGen.Next(1, 5);
                            if (spawnSide == 1)
                            {
                                x = ranGen.Next(0, 650);
                                y = ranGen.Next(-300, 50);
                                basicTexture = ramEnemyDown;
                            }
                            if (spawnSide == 2)
                            {
                                x = ranGen.Next(0, 650);
                                y = ranGen.Next(600, 1000);
                                basicTexture = ramEnemyUp;
                            }
                            if (spawnSide == 3)
                            {
                                x = ranGen.Next(-300, 50);
                                y = ranGen.Next(0, 650);
                                basicTexture = ramEnemyRight;
                            }
                            if (spawnSide == 4)
                            {
                                x = ranGen.Next(600, 1000);
                                y = ranGen.Next(0, 650);
                                basicTexture = ramEnemyLeft;
                            }
                            //Adds Bad Guy To The Wave List
                            ramEnemyList.Add(new RamEnemy(new Rectangle(x, y, 60, 60), basicTexture, Convert.ToInt32(System.Math.Round((fiveWaveCount + 1)))));
                        }
                    }
                }

                //Used To Stop Over Hitting
                bool clickedOne = false;

                //Spawns bullets if defenders are there and delay is 0
                if (defenderCount == 1 && bulletSpawnDelay <= 0)
                {
                    bulletList.Add(new Bullet(new Rectangle(470, 350, 10, 10), bulletTexture));
                    bulletSpawnDelay = 120;
                }
                else if (defenderCount == 2 && bulletSpawnDelay <= 0)
                {
                    bulletList.Add(new Bullet(new Rectangle(470, 350, 10, 10), bulletTexture));
                    bulletList.Add(new Bullet(new Rectangle(220, 325, 10, 10), bulletTexture));
                    bulletSpawnDelay = 120;
                }
                else if (defenderCount == 3 && bulletSpawnDelay <= 0)
                {
                    bulletList.Add(new Bullet(new Rectangle(460, 345, 10, 10), bulletTexture));
                    bulletList.Add(new Bullet(new Rectangle(220, 325, 10, 10), bulletTexture));
                    bulletList.Add(new Bullet(new Rectangle(330, 440, 10, 10), bulletTexture));
                    bulletSpawnDelay = 120;
                }
                else if (defenderCount == 4 && bulletSpawnDelay <= 0)
                {
                    bulletList.Add(new Bullet(new Rectangle(460, 345, 10, 10), bulletTexture));
                    bulletList.Add(new Bullet(new Rectangle(220, 325, 10, 10), bulletTexture));
                    bulletList.Add(new Bullet(new Rectangle(330, 440, 10, 10), bulletTexture));
                    bulletList.Add(new Bullet(new Rectangle(355, 195, 10, 10), bulletTexture));
                    bulletSpawnDelay = 120;
                }
                else if (defenderCount > 0 && bulletSpawnDelay >= 0)
                {
                    bulletSpawnDelay--;
                }

                foreach (Bullet i in bulletList)
                {
                    if (ramEnemyList.Count != 0)
                    {
                        i.Move(_graphics, ramEnemyList[0].BoundRect);
                    }
                    else if (reinforcedEnemyList.Count != 0)
                    {
                        i.Move(_graphics, reinforcedEnemyList[0].BoundRect);
                    }
                    else if (basicEnemys.Count != 0)
                    {
                        i.Move(_graphics, basicEnemys[0].BoundRect);
                    }
                }

                //Loops Through Bad Guy List
                for (int i = 0; i < basicEnemys.Count; i++)
                {
                    //Moves The Bad Guys And Checks If They Hit The Castle, If They Did, It Sets The Castle Health To Its New Value
                    castleHealth = basicEnemys[i].Move(_graphics, castleRect, castleHealth);

                    //Checks If The Blades Hits, Applies Damage
                    foreach (Blade blade in bladesList)
                    {
                        if (blade.BoundRect.Intersects(basicEnemys[i].BoundRect))
                        {
                            basicEnemys[i].Damage(1);
                        }
                    }
                    //Checks if bullet hits, deals damage and then removes bullet
                    for (int e = 0; e < bulletList.Count; e++)
                    {
                        if (bulletList[e].BoundRect.Intersects(basicEnemys[i].BoundRect))
                        {
                            basicEnemys[i].Damage(atkDamage);
                            bulletList.RemoveAt(e);
                            e--;
                        }

                    }
                    // Detects A Click on Enemies, Applies Damage
                    if (!clickedOne && mouseState.LeftButton == ButtonState.Pressed && preMouseState.LeftButton == ButtonState.Released)
                    {
                        if (basicEnemys[i].BoundRect.Contains(mouseState.X, mouseState.Y))
                        {
                            clickedOne = true;
                            basicEnemys[i].Damage(atkDamage);

                        }
                    }
                    //Checks If A Enemy Dies, Gives Coins
                    if (basicEnemys[i].Health <= 0)
                    {
                        basicEnemys.RemoveAt(i);
                        coins += ranGen.Next(1, 4);
                        i--;
                    }
                }

                //Loops through reinforced bad guys
                for (int i = 0; i < reinforcedEnemyList.Count; i++)
                {
                    //Moves The Bad Guys And Checks If They Hit The Castle, If They Did, It Sets The Castle Health To Its New Value
                    castleHealth = reinforcedEnemyList[i].Move(_graphics, castleRect, castleHealth);

                    // Detects A Click on Enemies, Applies Damage
                    if (!clickedOne && mouseState.LeftButton == ButtonState.Pressed && preMouseState.LeftButton == ButtonState.Released)
                    {
                        if (reinforcedEnemyList[i].BoundRect.Contains(mouseState.X, mouseState.Y))
                        {
                            clickedOne = true;
                            reinforcedEnemyList[i].Damage(atkDamage);

                        }
                    }
                    //Checks if bullet hits, deals damage and then removes bullet
                    for (int e = 0; e < bulletList.Count; e++)
                    {
                        if (bulletList[e].BoundRect.Intersects(reinforcedEnemyList[i].BoundRect))
                        {
                            reinforcedEnemyList[i].Damage(atkDamage);
                            bulletList.RemoveAt(e);
                            e--;
                        }

                    }
                    //Checks If A Enemy Dies, Gives Coins
                    if (reinforcedEnemyList[i].Health <= 0)
                    {
                        reinforcedEnemyList.RemoveAt(i);
                        coins += ranGen.Next(5, 12);
                        i--;
                    }
                }

                //Loops through ram bad guys
                for (int i = 0; i < ramEnemyList.Count; i++)
                {
                    //Moves The Bad Guys And Checks If They Hit The Castle, If They Did, It Sets The Castle Health To Its New Value
                    castleHealth = ramEnemyList[i].Move(_graphics, castleRect, castleHealth);

                    // Detects A Click on Enemies, Applies Damage
                    if (!clickedOne && mouseState.LeftButton == ButtonState.Pressed && preMouseState.LeftButton == ButtonState.Released)
                    {
                        if (ramEnemyList[i].BoundRect.Contains(mouseState.X, mouseState.Y))
                        {
                            clickedOne = true;
                            ramEnemyList[i].Damage(atkDamage);

                        }
                    }
                    //Checks if bullet hits, deals damage and then removes bullet
                    for (int e = 0; e < bulletList.Count; e++)
                    {
                        if (bulletList[e].BoundRect.Intersects(ramEnemyList[i].BoundRect))
                        {
                            ramEnemyList[i].Damage(atkDamage);
                            bulletList.RemoveAt(e);
                            e--;
                        }

                    }
                    //Checks If The Blades Hits, Applies Damage
                    foreach (Blade blade in bladesList)
                    {
                        if (blade.BoundRect.Intersects(ramEnemyList[i].BoundRect))
                        {
                            ramEnemyList[i].Damage(1);
                        }
                    }
                    //Checks If A Enemy Dies, Gives Coins
                    if (ramEnemyList[i].Health <= 0)
                    {
                        ramEnemyList.RemoveAt(i);
                        coins += ranGen.Next(5, 12);
                        i--;
                    }
                }

                //Moves The Blades
                foreach (Blade blade in bladesList)
                {
                    blade.Move(_graphics);
                }
                //Allows The User To Move From The Game Screen To The Shop Screen By Clicking The Shop Button
                if (mouseState.LeftButton == ButtonState.Pressed && preMouseState.LeftButton == ButtonState.Released)
                {
                    if (shopButtonRect.Contains(mouseState.X, mouseState.Y))
                    {
                        screen = Screen.ShopScreen;
                    }
                }
            }
            //How To Play Screen
            else if (screen == Screen.HowToPlay)
            {

            }
            //The Shop Screen
            else if (screen == Screen.ShopScreen)
            {
                //Checks For Mouse Button Click
                if (mouseState.LeftButton == ButtonState.Pressed && preMouseState.LeftButton == ButtonState.Released)
                {
                    //Checks If The User Clicks The Shop Button, Moves Back To The Game Screen
                    if (shopButtonRect.Contains(mouseState.X, mouseState.Y))
                    {
                        screen = Screen.GameScreen;
                    }
                    //Checks If The User Buys More Attack Damage, Does Math To See If They Can, Then Gives Them What They Bought, Also Increases Price For Next Time
                    if (atkBuyRect.Contains(mouseState.X, mouseState.Y))
                    {
                        int cost = atkDamage * 10;
                        if (coins >= cost)
                        {
                            atkDamage++;
                            coins -= cost;
                        }
                    }
                    //Checks If The User Buys More Blades, Does Math To See If They Can, Then Gives Them What They Bought, Also Increases Price For Next Time
                    if (bladeBuyRect.Contains(mouseState.X, mouseState.Y))
                    {
                        int cost = (bladeCount * 100) + 100;
                        if (coins >= cost)
                        {
                            bladeCount++;
                            int xSpeed = ranGen.Next(1, 3);
                            int ySpeed = ranGen.Next(1, 3);
                            if (xSpeed == 1)
                                xSpeed = -2;
                            if (xSpeed == 2)
                                xSpeed = 2;
                            if (ySpeed == 1)
                                ySpeed = -2;
                            if (ySpeed == 2)
                                ySpeed = 2;
                            bladesList.Add(new Blade(new Rectangle(ranGen.Next(100, 600), ranGen.Next(100, 600), 30, 30), new Vector2(xSpeed, ySpeed), sawBladeTexture));
                            coins -= cost;
                        }
                    }
                    //Checks If The User Buys More Defenders, Does Math To See If They Can, Then Gives Them What They Bought, Also Increases Price For Next Time
                    if (defenderBuyRect.Contains(mouseState.X, mouseState.Y) && defenderCount < 4)
                    {
                        int cost = (defenderCount * 100) + 200;
                        if (coins >= cost)
                        {
                            defenderCount++;
                            coins -= cost;
                        }
                    }
                }
            }
            //The End Screen
            else if (screen == Screen.EndScreen)
            {

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            //Main Screen
            if (screen == Screen.MainMenu)
            {
                //Draws All The Things Needed In The Main Screen
                _spriteBatch.Draw(menuBackground, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
                _spriteBatch.Draw(playButton, playButtonRect, Color.White);
                _spriteBatch.Draw(howToPlayTexture, howToPlayRect, Color.White);
                _spriteBatch.Draw(gameTitle, new Rectangle(100, 250, 500, 100), Color.White);
            }

            //Game Screen
            else if (screen == Screen.GameScreen)
            {
                //Draws All The Things Needed In The Game Screen
                _spriteBatch.Draw(gameBackground, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
                _spriteBatch.Draw(castleTexture, castleRect, Color.White);
                _spriteBatch.Draw(heartIcon, new Rectangle(0, 0, 25, 25), Color.White);
                _spriteBatch.DrawString(castleHealthText, $"{System.Math.Round(castleHealth, 2)}", new Vector2(30, 0), Color.Red);
                _spriteBatch.Draw(coinIcon, new Rectangle(0, 32, 25, 25), Color.White);
                _spriteBatch.DrawString(castleHealthText, $"{coins}", new Vector2(30, 30), Color.Gold);
                _spriteBatch.Draw(shopIcon, shopButtonRect, Color.White);
                _spriteBatch.Draw(swordIcon, new Rectangle(0, 64, 22, 22), Color.White);
                _spriteBatch.DrawString(castleHealthText, $"{atkDamage}", new Vector2(30, 60), Color.LightGray);
                _spriteBatch.Draw(sawBladeTexture, new Rectangle(0, 94, 22, 22), Color.White);
                _spriteBatch.DrawString(castleHealthText, $"{bladeCount}", new Vector2(30, 90), Color.LightGray);
                _spriteBatch.Draw(defenderIcon, new Rectangle(5, 124, 22, 22), Color.White);
                _spriteBatch.DrawString(castleHealthText, $"{defenderCount}", new Vector2(30, 120), Color.LightGray);
                if (defenderCount == 1)
                {
                    _spriteBatch.Draw(defenderRight, new Rectangle(430, 325, 50, 50), Color.White);
                }
                else if (defenderCount == 2)
                {
                    _spriteBatch.Draw(defenderRight, new Rectangle(430, 325, 50, 50), Color.White);
                    _spriteBatch.Draw(defenderLeft, new Rectangle(220, 325, 50, 50), Color.White);
                }
                else if (defenderCount == 3)
                {
                    _spriteBatch.Draw(defenderRight, new Rectangle(430, 325, 50, 50), Color.White);
                    _spriteBatch.Draw(defenderLeft, new Rectangle(220, 325, 50, 50), Color.White);
                    _spriteBatch.Draw(defenderDown, new Rectangle(325, 400, 50, 50), Color.White);
                }
                else if (defenderCount == 4)
                {
                    _spriteBatch.Draw(defenderRight, new Rectangle(430, 325, 50, 50), Color.White);
                    _spriteBatch.Draw(defenderLeft, new Rectangle(220, 325, 50, 50), Color.White);
                    _spriteBatch.Draw(defenderDown, new Rectangle(325, 400, 50, 50), Color.White);
                    _spriteBatch.Draw(defenderUp, new Rectangle(325, 195, 50, 50), Color.White);
                }
                foreach (BasicEnemy enemyB in basicEnemys)
                {
                    enemyB.Draw(_spriteBatch, badGuyHealthText);
                }
                foreach (ReinforcedEnemy enemyB in reinforcedEnemyList)
                {
                    enemyB.Draw(_spriteBatch, badGuyHealthText);
                }
                foreach (RamEnemy enemyB in ramEnemyList)
                {
                    enemyB.Draw(_spriteBatch, badGuyHealthText);
                }
                foreach (Blade blade in bladesList)
                {
                    blade.Draw(_spriteBatch);
                }
                foreach (Bullet bullet in bulletList)
                {
                    bullet.Draw(_spriteBatch);
                }
                _spriteBatch.DrawString(castleHealthText, $"Wave: {wave}", new Vector2(300, 0), Color.Black);
            }

            //Shop Screen
            else if (screen == Screen.ShopScreen)
            {
                //Draws All The Things Needed In The Shop Screen
                _spriteBatch.Draw(gameBackground, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
                _spriteBatch.Draw(castleTexture, castleRect, Color.White);
                _spriteBatch.Draw(heartIcon, new Rectangle(0, 0, 25, 25), Color.White);
                _spriteBatch.DrawString(castleHealthText, $"{System.Math.Round(castleHealth, 2)}", new Vector2(30, 0), Color.Red);
                _spriteBatch.Draw(coinIcon, new Rectangle(0, 32, 25, 25), Color.White);
                _spriteBatch.DrawString(castleHealthText, $"{coins}", new Vector2(30, 30), Color.Gold);
                _spriteBatch.Draw(shopIcon, shopButtonRect, Color.White);
                _spriteBatch.Draw(swordIcon, new Rectangle(0, 64, 22, 22), Color.White);
                _spriteBatch.DrawString(castleHealthText, $"{atkDamage}", new Vector2(30, 60), Color.LightGray);
                _spriteBatch.Draw(sawBladeTexture, new Rectangle(0, 94, 22, 22), Color.White);
                _spriteBatch.DrawString(castleHealthText, $"{bladeCount}", new Vector2(30, 90), Color.LightGray);
                _spriteBatch.Draw(defenderIcon, new Rectangle(5, 124, 22, 22), Color.White);
                _spriteBatch.DrawString(castleHealthText, $"{defenderCount}", new Vector2(30, 120), Color.LightGray);
                if (defenderCount == 1)
                {
                    _spriteBatch.Draw(defenderRight, new Rectangle(430, 325, 50, 50), Color.White);
                }
                else if (defenderCount == 2)
                {
                    _spriteBatch.Draw(defenderRight, new Rectangle(430, 325, 50, 50), Color.White);
                    _spriteBatch.Draw(defenderLeft, new Rectangle(220, 325, 50, 50), Color.White);
                }
                else if (defenderCount == 3)
                {
                    _spriteBatch.Draw(defenderRight, new Rectangle(430, 325, 50, 50), Color.White);
                    _spriteBatch.Draw(defenderLeft, new Rectangle(220, 325, 50, 50), Color.White);
                    _spriteBatch.Draw(defenderDown, new Rectangle(325, 400, 50, 50), Color.White);
                }
                else if (defenderCount == 4)
                {
                    _spriteBatch.Draw(defenderRight, new Rectangle(430, 325, 50, 50), Color.White);
                    _spriteBatch.Draw(defenderLeft, new Rectangle(220, 325, 50, 50), Color.White);
                    _spriteBatch.Draw(defenderDown, new Rectangle(325, 400, 50, 50), Color.White);
                    _spriteBatch.Draw(defenderUp, new Rectangle(325, 195, 50, 50), Color.White);
                }
                foreach (BasicEnemy enemyB in basicEnemys)
                {
                    enemyB.Draw(_spriteBatch, badGuyHealthText);
                }
                foreach (ReinforcedEnemy enemyB in reinforcedEnemyList)
                {
                    enemyB.Draw(_spriteBatch, badGuyHealthText);
                }
                foreach (RamEnemy enemyB in ramEnemyList)
                {
                    enemyB.Draw(_spriteBatch, badGuyHealthText);
                }
                foreach (Blade blade in bladesList)
                {
                    blade.Draw(_spriteBatch);
                }
                foreach (Bullet bullet in bulletList)
                {
                    bullet.Draw(_spriteBatch);
                }
                _spriteBatch.Draw(shopTint, new Rectangle(100, 0, 520, 100), Color.White);

                //Atk Buy Part, Text Turns Red If They Can't Buy It, Green If They Can
                _spriteBatch.Draw(swordIcon, atkBuyRect, Color.White);
                _spriteBatch.DrawString(shopText, $"{atkDamage} -> {atkDamage + 1}", new Vector2(140, 0), Color.LightGray);
                if (coins >= atkDamage * 10)
                    _spriteBatch.DrawString(shopText, $"${atkDamage * 10}", new Vector2(140, 76), Color.Green);
                else
                    _spriteBatch.DrawString(shopText, $"${atkDamage * 10}", new Vector2(140, 76), Color.Red);

                //Blade Buy Part, Text Turns Red If They Can't Buy It, Green If They Can
                _spriteBatch.Draw(sawBladeTexture, bladeBuyRect, Color.White);
                _spriteBatch.DrawString(shopText, $"{bladeCount} -> {bladeCount + 1}", new Vector2(240, 0), Color.LightGray);
                if (coins >= (bladeCount * 100) + 100)
                    _spriteBatch.DrawString(shopText, $"${(bladeCount * 100) + 100}", new Vector2(240, 76), Color.Green);
                else
                    _spriteBatch.DrawString(shopText, $"${(bladeCount * 100) + 100}", new Vector2(240, 76), Color.Red);

                //Defender Buy Part, Text Turns Red If They Can't Buy It, Green If They Can
                _spriteBatch.Draw(defenderIcon, defenderBuyRect, Color.White);
                _spriteBatch.DrawString(shopText, $"{defenderCount}/4", new Vector2(350, 0), Color.LightGray);
                if (defenderCount < 4)
                {
                    if (coins >= (defenderCount * 100) + 200)
                        _spriteBatch.DrawString(shopText, $"${(defenderCount * 100) + 200}", new Vector2(340, 76), Color.Green);
                    else
                        _spriteBatch.DrawString(shopText, $"${(defenderCount * 100) + 200}", new Vector2(340, 76), Color.Red);
                }
                else
                {
                    _spriteBatch.DrawString(shopText, $"N/A", new Vector2(340, 76), Color.LightGray);
                }
            }

            //End Screen
            else if (screen == Screen.EndScreen)
            {

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}