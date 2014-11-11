using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace TheBlindMan
{
    class CarManager
    {
        private List<Car> cars;
        private List<Car> removedCars;
        private List<Point> spawnLocations;
        private Random random = new Random();

        private SoundEffect soundEffect;
        private List<SoundEffect> soundEffects;

        private List<Car> premadeCars;

        public CarManager()
        {
            cars = new List<Car>();
            removedCars = new List<Car>();
            spawnLocations = new List<Point>();
            soundEffects = new List<SoundEffect>();

            premadeCars = new List<Car>();

            Point spawn1 = new Point(-130, 280);
            Point spawn2 = new Point(-130, 355);
            Point spawn3 = new Point(-130, 430);
            Point spawn4 = new Point(-130, 490);
            Point spawn5 = new Point(1210, 580);
            Point spawn6 = new Point(1210, 680);
            Point spawn7 = new Point(1210, 780);
            Point spawn8 = new Point(1210, 855);

            spawnLocations.Add(spawn1);
            spawnLocations.Add(spawn2);
            spawnLocations.Add(spawn3);
            spawnLocations.Add(spawn4);
            spawnLocations.Add(spawn5);
            spawnLocations.Add(spawn6);
            spawnLocations.Add(spawn7);
            spawnLocations.Add(spawn8);
        }

        public virtual void LoadContent(ContentManager content)
        {
            Animation carAnim = new Animation(content.Load<Texture2D>(@"Images/Cars/car_sheet"),
               new Point(128, 40), new Point(0, 0), new Point(2, 1), 600);
            Car car = new Car(carAnim, 20);
            premadeCars.Add(car);

            soundEffects.Add(content.Load<SoundEffect>(@"Audio/carSound1"));
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (Car car in this.cars)
            {
                car.Update(gameTime);
                if (car.X  < -300 || car.X > 1300)
                {
                    removedCars.Add(car);
                }
            }
            foreach (Car car in this.removedCars)
            {
                cars.Remove(car);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Car car in this.cars)
            {
                car.Draw(gameTime, spriteBatch);
            }
        }

        public void AddCar(int numberOfCars = 1)
        {
            if (numberOfCars <= 0)
                return;
            for (int index = 0; index < numberOfCars; index++)
            {
                cars.Add(GenerateCar());
            }
        }

        private Car GenerateCar()
        {
            //int animationIndex = random.Next(0, animations.Count);
            int premadeCarIndex = random.Next(0, premadeCars.Count);
            int spawnIndex = random.Next(0, spawnLocations.Count);
            int soundIndex = random.Next(0, soundEffects.Count);

            float speed = (float)random.Next(10, 30);
            if (spawnLocations[spawnIndex].X <= 0)
            {
                speed *= 1;
            }
            else
            {
                speed *= -1;
            }

            Car newCar = new Car(premadeCars[premadeCarIndex]);
            newCar.X = (float)spawnLocations[spawnIndex].X;
            newCar.Y = (float)spawnLocations[spawnIndex].Y;
            newCar.Speed = speed;
            newCar.SoundEffect = soundEffects[soundIndex];

            return newCar;
        }

        public int CarCount()
        {
            return cars.Count;
        }
    }
}
