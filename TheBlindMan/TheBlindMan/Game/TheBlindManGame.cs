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
        PauseScreen pauseScreen;

        Camera camera = new Camera();
        Vector2 camPos = new Vector2(0,0);
        Vector2 camSpeed = new Vector2(0,1);
        Vector3 screenScale = new Vector3(1080f, 720f, 1f);

        #region Getter(s) Setter(s)
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
            set { playScreen = value; }
        }

        public InfoScreen InfoScreen
        {
            get { return infoScreen; }
        }

        public CreditScreen CreditScreen
        {
            get { return creditScreen; }
        }

        public PauseScreen PauseScreen
        {
            get { return pauseScreen; }
        }
        #endregion

        public TheBlindManGame()
        {
            graphics = new GraphicsDeviceManager(this);
            TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 16);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1080;
            Content.RootDirectory = "Content";
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
            startScreen = new StartScreen(this);
            startScreen.LoadContent(Content);
            Components.Add(startScreen);

            playScreen = new PlayScreen(this);
            playScreen.LoadContent(Content);
            playScreen.Initialize();
            Components.Add(playScreen);

            infoScreen = new InfoScreen(this);
            infoScreen.LoadContent(Content);
            Components.Add(infoScreen);

            creditScreen = new CreditScreen(this);
            creditScreen.LoadContent(Content);
            Components.Add(creditScreen);

            pauseScreen = new PauseScreen(this);
            pauseScreen.LoadContent(Content);
            Components.Add(pauseScreen);

            activeScreen = StartScreen;
            activeScreen.Start();
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

            int playerCenter = Players.OldMan.Bounds.Center.Y;
            if (playerCenter >= 1080)
                camPos.Y = 720;
            else if (playerCenter < 1080 && playerCenter > 360)
                camPos.Y = playerCenter - 360;
            else if (playerCenter <= 360)
                camPos.Y = 0;
            camPos.Y += camSpeed.Y;
            camera.Update(camPos);
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

            if (activeScreen == StartScreen || activeScreen == PauseScreen)
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                null, null, null, null, camera.ViewMatrix);
            else 
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, 
                    null, null, null, null, camera.ViewMatrix);
            base.Draw(gameTime);

            activeScreen.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
        #endregion
    }
}
