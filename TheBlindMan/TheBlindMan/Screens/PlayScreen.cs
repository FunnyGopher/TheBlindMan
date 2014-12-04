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
        private List<Car> parkedCars;

        public PlayScreen(TheBlindManGame game)
            : base(game)
        {
            winZone = new Rectangle(0, 0, 1080, 129);
            carFactory = new CarFactory();
            parkedCars = new List<Car>();
        }

        public void Initialize()
        {
            Players.OldMan.Scale = 1.2f;
            Players.Dog.Scale = 1.2f;

            Console.WriteLine("Makin' some lanes");

            List<Lane> carLanes = new List<Lane>();

            // These lanes are the top 4 lanes
            carLanes.Add(new Lane(new Point(1400, 252), new Point(-400, 252), 10, 10, 1, 3));
            carLanes.Add(new Lane(new Point(1400, 321), new Point(-400, 321), 15, 15));
            carLanes.Add(new Lane(new Point(1400, 390), new Point(-400, 390), 20, 35));
            carLanes.Add(new Lane(new Point(1400, 456), new Point(-400, 456), 25, 55));

            carLanes.Add(new Lane(new Point(-400, 536), new Point(1400, 536), 25, 60));
            carLanes.Add(new Lane(new Point(-400, 604), new Point(1400, 604), 20, 40));
            carLanes.Add(new Lane(new Point(-400, 671), new Point(1400, 671), 15, 20));
            carLanes.Add(new Lane(new Point(-400, 737), new Point(1400, 737), 10, 10, 1, 3));            

            carFactory.AddLanes(carLanes);

            parkedCars.Add(carFactory.GenerateParkedCar(130, 1040));
            parkedCars.Add(carFactory.GenerateParkedCar(310, 1175));
            parkedCars.Add(carFactory.GenerateParkedCar(560, 1172));
            parkedCars.Add(carFactory.GenerateParkedCar(310, 968));
            parkedCars.Add(carFactory.GenerateParkedCar(560, 1032));
            parkedCars.Add(carFactory.GenerateParkedCar(310, 1376));
            parkedCars.Add(carFactory.GenerateParkedCar(560, 1304));
            parkedCars.Add(carFactory.GenerateParkedCar(560, 1376));

            parkedCars.Add(carFactory.GenerateParkedCar(960, 1040));
            parkedCars.Add(carFactory.GenerateParkedCar(960, 1304));
            parkedCars.Add(carFactory.GenerateParkedCar(-80, 1240));
        }

        public override void LoadContent(ContentManager content)
        {
            this.bgImage = content.Load<Texture2D>(@"Images/Backgrounds/Level");
            this.bgSound = content.Load<SoundEffect>(@"Audio/trafAmbi");
            this.bgSoundInstance = bgSound.CreateInstance();

            Players.OldMan = new OldMan(PlayerIndex.One, this);
            Players.Dog = new Dog(PlayerIndex.Two);

            Players.OldMan.LoadContent(content);
            Players.Dog.LoadContent(content);

            carFactory.LoadContent(content);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            /*
            if (Keyboard.GetState().IsKeyDown(Keys.P) ||
                GamePad.GetState(Players.OldMan.PlayerIndex).Buttons.Start == ButtonState.Pressed)
                Game.ActiveScreen = Game.PauseScreen;
            */

            carFactory.Update(gameTime);

            foreach (Car car in parkedCars)
                car.Update(gameTime);

            Players.OldMan.Update(gameTime);
            Players.Dog.Update(gameTime);

            if (float.IsNaN(Players.OldMan.X) || float.IsNaN(Players.OldMan.Y))
                InitialSpawn();

            if (winZone.Intersects(Players.OldMan.Bounds))
                Game.ActiveScreen = Game.StartScreen;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bgImage, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1f); 

            DrawPlayers(gameTime, spriteBatch);
            carFactory.Draw(gameTime, spriteBatch);
            foreach (Car car in parkedCars)
                car.Draw(gameTime, spriteBatch);

            base.Draw(gameTime, spriteBatch);
        }

        private void DrawPlayers(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Players.OldMan.Draw(gameTime, spriteBatch);
            Players.Dog.Draw(gameTime, spriteBatch);
        }

        private void InitialSpawn()
        {
            Players.OldMan.X = 190;
            Players.OldMan.Y = 1360;

            Players.Dog.X = 230;
            Players.Dog.Y = 1390;
        }

        private void PlayBackgroundSound()
        {
            bgSoundInstance = bgSound.CreateInstance();
            bgSoundInstance.Play();
        }

        public override void Start()
        {
            InitialSpawn();
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
