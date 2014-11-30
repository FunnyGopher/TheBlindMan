using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace TheBlindMan
{
    public class PlayScreen : GameScreen
    {
        private Texture2D bgImage;
        private SoundEffect bgSound;
        private SoundEffectInstance bgSoundInstance;

        private CarManager carManager;
        private Rectangle winZone;

        public PlayScreen(TheBlindManGame game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
            Players.OldMan = new OldMan(PlayerIndex.One);
            Players.Dog = new Dog(PlayerIndex.Two);

            List<Point> spawnPoints = new List<Point>();
            spawnPoints.Add(new Point(-130, 280));
            spawnPoints.Add(new Point(-130, 355));
            spawnPoints.Add(new Point(-130, 430));
            spawnPoints.Add(new Point(-130, 490));
            spawnPoints.Add(new Point(1210, 580));
            spawnPoints.Add(new Point(1210, 680));
            spawnPoints.Add(new Point(1210, 780));
            spawnPoints.Add(new Point(1210, 855));

            carManager = new CarManager(game);
            carManager.AddSpawnPoints(spawnPoints);

            winZone = new Rectangle(460, 100, 60, 40);
        }

        public override void LoadContent(ContentManager content)
        {
            this.bgImage = content.Load<Texture2D>(@"Images/Backgrounds/Level");
            this.bgSound = content.Load<SoundEffect>(@"Audio/trafAmbi");
            this.bgSoundInstance = bgSound.CreateInstance();

            Players.OldMan.LoadContent(content);
            Players.Dog.LoadContent(content);

            carManager.LoadContent(content);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape) ||
                GamePad.GetState(Players.OldMan.PlayerIndex).Buttons.Back == ButtonState.Pressed)
                game.ActiveScreen = game.StartScreen;

            carManager.Update(gameTime);
            if (carManager.Count < 10)
                carManager.AddCar();

            Players.OldMan.Update(gameTime);
            Players.Dog.Update(gameTime);

            if (winZone.Intersects(Players.OldMan.Bounds))
                game.ActiveScreen = game.StartScreen;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(bgImage, new Vector2(0,0), Color.White);
            base.Draw(gameTime);

            DrawPlayers(gameTime, spriteBatch);
            carManager.Draw(gameTime, spriteBatch);
        }

        private void DrawPlayers(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Players.OldMan.Draw(gameTime, spriteBatch);
            Players.Dog.Draw(gameTime, spriteBatch);
        }

        private void SpawnPlayers()
        {
            Players.OldMan.X = 540;
            Players.OldMan.Y = 1400;

            Players.Dog.X = 500;//570;
            Players.Dog.Y = 1400;//920;
        }

        private void PlayBackgroundSound()
        {
            bgSoundInstance = bgSound.CreateInstance();
            bgSoundInstance.Play();
        }

        public override void Start()
        {
            SpawnPlayers();
            PlayBackgroundSound();
            base.Start();
        }

        public override void Stop()
        {
            if (bgSoundInstance.State == SoundState.Playing)
                bgSoundInstance.Stop();

            bgSoundInstance.Dispose();
            carManager.Clear();
            base.Stop();
        }
    }
}
