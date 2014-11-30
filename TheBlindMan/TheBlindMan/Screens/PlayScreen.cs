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
            carManager = new CarManager(game);
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

            Players.OldMan.Update(gameTime);
            Players.Dog.Update(gameTime);

            if (winZone.Intersects(Players.OldMan.Bounds))
                game.ActiveScreen = game.StartScreen;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(bgImage, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1); 
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
            Players.OldMan.Y = 1350;

            Players.Dog.X = 510;
            Players.Dog.Y = 1380;
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
