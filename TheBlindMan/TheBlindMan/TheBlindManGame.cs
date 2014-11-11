#region File Information
/*
 * 
 */
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace TheBlindMan
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TheBlindManGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;
        GamePadState gamePadState;
        GamePadState oldGamePadState;

        GameScreen activeScreen;
        StartScreen startScreen;
        InfoScreen infoScreen;
        CreditScreen creditScreen;
        PlayScreen playScreen;

        public GameScreen ActiveScreen
        {
            get { return activeScreen; }
            set
            {
                activeScreen.Hide();
                activeScreen = value;
                activeScreen.Show();
            }
        }

        public StartScreen StartScreen
        {
            get { return startScreen; }
        }

        public TheBlindManGame()
        {
            graphics = new GraphicsDeviceManager(this);
            TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 16);
            graphics.PreferredBackBufferHeight = 1000;
            graphics.PreferredBackBufferWidth = 1080;
            Content.RootDirectory = "Content";
            Console.WriteLine("Loading Content from the game");
        }

        #region Initialize
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }
        #endregion

        private void InitializePlayers()
        {
            Players.OldMan = new OldMan(PlayerIndex.One, 300, 150);
            Players.Dog = new Dog(PlayerIndex.Two, 300, 150);
        }

        #region Load Content
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            startScreen = new StartScreen(this,
                spriteBatch,
                Content.Load<SpriteFont>("Font/menuFont"),
                Content.Load<Texture2D>("Images/Backgrounds/Title"));
            Components.Add(startScreen);
            startScreen.Hide();

            playScreen = new PlayScreen(this, spriteBatch);
            playScreen.LoadContent(Content);
            Components.Add(playScreen);
            playScreen.Hide();

            
            infoScreen = new InfoScreen(this, spriteBatch,
                Content.Load<Texture2D>("Images/Backgrounds/Controls"));
            Components.Add(infoScreen);
            infoScreen.Hide();

            creditScreen = new CreditScreen(this, spriteBatch,
                Content.Load<Texture2D>("Images/Backgrounds/Credits"));
            Components.Add(creditScreen);
            creditScreen.Hide();

            activeScreen = startScreen;
            activeScreen.Show();
        }
        #endregion

        #region Unload Content
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }
        #endregion

        #region Update
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);

            HandleStartScreen();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
            oldKeyboardState = keyboardState;
            oldGamePadState = gamePadState;
        }
        #endregion

        private void HandleStartScreen()
        {
            if (activeScreen == startScreen)
            {
                if (CheckKey(Keys.Enter) || CheckButton(Buttons.A))
                {
                    switch (startScreen.SelectedIndex)
                    {
                        case 0:
                            activeScreen.Hide();
                            activeScreen = playScreen;
                            activeScreen.Show();
                            break;
                        case 1:
                            activeScreen.Hide();
                            activeScreen = infoScreen;
                            activeScreen.Show();
                            break;
                        case 2:
                            activeScreen.Hide();
                            activeScreen = creditScreen;
                            activeScreen.Show();
                            break;
                        case 3:
                            this.Exit();
                            break;
                    }
                }
            }
            else
            {
                if (CheckKey(Keys.Escape) || CheckButton(Buttons.Back))
                {
                    activeScreen.Hide();
                    activeScreen = startScreen;
                    activeScreen.Show();
                }
            }
        }

        private void HandlePlayScreen()
        {

        }

        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) && oldKeyboardState.IsKeyDown(theKey);
        }

        private bool CheckButton(Buttons theButton)
        {
            return gamePadState.IsButtonUp(theButton) && oldGamePadState.IsButtonDown(theButton);
        }

        #region Draw
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.End();

            
        }
        #endregion
    }
}
