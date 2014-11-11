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
                activeScreen.Stop();
                activeScreen = value;
                activeScreen.Start();
            }
        }

        public StartScreen StartScreen
        {
            get { return startScreen; }
        }

        public PlayScreen PlayScreen
        {
            get { return playScreen; }
        }

        public InfoScreen InfoScreen
        {
            get { return infoScreen; }
        }

        public CreditScreen CreditScreen
        {
            get { return creditScreen; }
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

            startScreen = new StartScreen(this, spriteBatch);
            startScreen.LoadContent(Content);
            Components.Add(startScreen);

            playScreen = new PlayScreen(this, spriteBatch);
            playScreen.LoadContent(Content);
            Components.Add(playScreen);

            infoScreen = new InfoScreen(this, spriteBatch);
            infoScreen.LoadContent(Content);
            Components.Add(infoScreen);

            creditScreen = new CreditScreen(this, spriteBatch);
            creditScreen.LoadContent(Content);
            Components.Add(creditScreen);

            activeScreen = StartScreen;
        }
        #endregion

        #region Load Content
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
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
            activeScreen.Update(gameTime);

            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            base.Update(gameTime);
        }
        #endregion

        

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
            activeScreen.Draw(gameTime);
            spriteBatch.End();
        }
        #endregion
    }
}
