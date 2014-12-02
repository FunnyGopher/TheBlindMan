using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBlindMan
{
    class CarFactory
    {
        private List<Lane> lanes;
        private Random random = new Random();
        private const int MAX_NUMBER_OF_CARS = 12;
        private const int TOTAL_TYPES_OF_CARS = 1;
        private List<SoundEffect> soundEffects;
        private Car[] preFabCars;

        public CarFactory()
        {
            lanes = new List<Lane>();
            soundEffects = new List<SoundEffect>();
            preFabCars = new Car[TOTAL_TYPES_OF_CARS];     
        }

        public Car GenerateCar()
        {
            Car car = new Car(preFabCars[random.Next(0, TOTAL_TYPES_OF_CARS)]);
            SoundEffect soundEffect = soundEffects[random.Next(0, soundEffects.Count)];
            car.SoundEffect = soundEffect;

            return car;
        }

        public virtual void LoadContent(ContentManager content)
        {
            Animation carAnim = new Animation(content.Load<Texture2D>(@"Images/Cars/car_sheet"),
               new Point(128, 40), new Point(0, 0), new Point(2, 1), 2000);
            Car car = new Car(carAnim, 20);
            preFabCars[0] = car;

            soundEffects.Add(content.Load<SoundEffect>(@"Audio/carSound1"));
        }

        public virtual void Update(GameTime gameTime)
        {
            int numberOfCars = 0;
            int randomNumb = 0;
            List<Lane> openLanes = new List<Lane>();

            foreach (Lane lane in lanes)
            {
                lane.Update(gameTime);
                if (lane.IsOpen())
                    openLanes.Add(lane);
                numberOfCars += lane.CurrentCarCount();
            }

            if (openLanes.Count != 0 && numberOfCars < MAX_NUMBER_OF_CARS)
            {
                randomNumb = random.Next(0, openLanes.Count - 1);
                AddCar(openLanes[randomNumb], GenerateCar());
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Lane lane in lanes)
                lane.Draw(gameTime, spriteBatch);
        }

        public void AddCar(Lane lane, Car car)
        {
            lane.AddCar(car);
        }

        public void AddLane(Lane lane)
        {
            lanes.Add(lane);
        }

        public void AddLanes(List<Lane> lanes)
        {
            foreach (Lane lane in lanes)
                this.lanes.Add(lane);
        }

        public void Clear()
        {
            foreach (Lane lane in lanes)
                lane.Clear();
        }
    }
}
