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
        private TheBlindManGame game;
        private List<Car> cars;
        private List<Car> carsToRemove;
        private List<Point> spawnLocations;
        private Random random = new Random();

        private List<SoundEffect> soundEffects;

        private Car[] premadeCars;

        public int Count
        {
            get
            {
                return cars.Count;
            }
        }

        public CarManager()
        {
            cars = new List<Car>();
            carsToRemove = new List<Car>();
            spawnLocations = new List<Point>();
            soundEffects = new List<SoundEffect>();
            premadeCars = new Car[1];

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

        private Car GenerateCar()
        {
            int premadeCarIndex = random.Next(0, premadeCars.Length);
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

            Car car = new Car(premadeCars[premadeCarIndex]);
            car.X = (float)spawnLocations[spawnIndex].X;
            car.Y = (float)spawnLocations[spawnIndex].Y;
            car.Speed = speed;
            car.SoundEffect = soundEffects[soundIndex];

            return car;
        }

        public virtual void LoadContent(ContentManager content)
        {
            Animation carAnim = new Animation(content.Load<Texture2D>(@"Images/Cars/car_sheet"),
               new Point(128, 40), new Point(0, 0), new Point(2, 1), 600);
            Car car = new Car(carAnim, 20);
            premadeCars[0] = car;

            soundEffects.Add(content.Load<SoundEffect>(@"Audio/carSound1"));
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (Car car in cars)
            {
                car.Update(gameTime);
                if (car.X + car.Bounds.Width < game.Window.ClientBounds.Left - car.Bounds.Width || car.X > game.Window.ClientBounds.Right + car.Bounds.Width)
                {
                    carsToRemove.Add(car);
                }
            }
            foreach (Car car in carsToRemove)
            {
                cars.Remove(car);
            }
            carsToRemove.Clear();
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Car car in cars)
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
    }
}
