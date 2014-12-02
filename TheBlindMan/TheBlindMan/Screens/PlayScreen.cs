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

        private CarFactory carFactory;
        private Rectangle winZone;
        private Car[] parkedCars;

        public PlayScreen(TheBlindManGame game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
            Players.OldMan = new OldMan(PlayerIndex.One);
            Players.Dog = new Dog(PlayerIndex.Two);

            winZone = new Rectangle(460, 100, 60, 40);
            carFactory = new CarFactory();
            parkedCars = new Car[1];
        }

        public void Initialize()
        {
            /*
            List<Point> carSpawnPoints = new List<Point>();
            carSpawnPoints.Add(new Point(-130, 486));
            carSpawnPoints.Add(new Point(1210, 199));
            carSpawnPoints.Add(new Point(-130, 554));
            carSpawnPoints.Add(new Point(1210, 272));
            carSpawnPoints.Add(new Point(-130, 624));
            carSpawnPoints.Add(new Point(1210, 339));
            carSpawnPoints.Add(new Point(-130, 691));
            carSpawnPoints.Add(new Point(1210, 408));
            carFactory.AddSpawnPoints(carSpawnPoints);
             * */

            List<Lane> carLanes = new List<Lane>();
            carLanes.Add(new Lane(new Point(-130, 486), new Point(1210, 486)));
            carFactory.AddLanes(carLanes);

            Car parkedCar = carFactory.GenerateCar();
            parkedCar.Animate = false;
            parkedCar.X = 300;
            parkedCar.Y = 800;
            parkedCar.Speed = 0;
            parkedCars[0] = parkedCar;
        }

        public override void LoadContent(ContentManager content)
        {
            this.bgImage = content.Load<Texture2D>(@"Images/Backgrounds/Level");
            this.bgSound = content.Load<SoundEffect>(@"Audio/trafAmbi");
            this.bgSoundInstance = bgSound.CreateInstance();

            Players.OldMan.LoadContent(content);
            Players.Dog.LoadContent(content);

            carFactory.LoadContent(content);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape) ||
                GamePad.GetState(Players.OldMan.PlayerIndex).Buttons.Back == ButtonState.Pressed)
                game.ActiveScreen = game.StartScreen;

            carFactory.Update(gameTime);

            foreach (Car car in parkedCars)
                car.Update(gameTime);

            Players.OldMan.Update(gameTime);
            Players.Dog.Update(gameTime);

            if (winZone.Intersects(Players.OldMan.Bounds))
                game.ActiveScreen = game.StartScreen;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(bgImage, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1f); 
            base.Draw(gameTime);

            DrawPlayers(gameTime, spriteBatch);
            carFactory.Draw(gameTime, spriteBatch);
            foreach (Car car in parkedCars)
                car.Draw(gameTime, spriteBatch);
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
            carFactory.Clear();
            base.Stop();
        }
    }
}
