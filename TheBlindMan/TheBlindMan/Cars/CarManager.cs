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
        private List<Point> spawnPoints;
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

        public CarManager(TheBlindManGame game)
        {
            this.game = game;

            cars = new List<Car>();
            carsToRemove = new List<Car>();
            spawnPoints = new List<Point>();
            soundEffects = new List<SoundEffect>();
            premadeCars = new Car[1];
        }

        private Car GenerateCar()
        {
            Car car = new Car(premadeCars[random.Next(0, premadeCars.Length)]);

            Point spawnPoint =  spawnPoints[random.Next(0, spawnPoints.Count)];
            car.X = spawnPoint.X;
            car.Y = spawnPoint.Y;
            Console.WriteLine("Hey");
            float speed = (float)random.Next(1, 2); //10,30;
            speed *= spawnPoint.X <= 0 ? 1 : -1;
            car.Speed = speed;

            SoundEffect soundEffect = soundEffects[random.Next(0, soundEffects.Count)];
            car.SoundEffect = soundEffect;

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
                if (car.X + car.Bounds.Width < 0 - car.Bounds.Width || car.X > game.GraphicsDevice.Viewport.Width + car.Bounds.Width)
                    carsToRemove.Add(car);
            }

            foreach (Car car in carsToRemove)
                cars.Remove(car);
            carsToRemove.Clear();
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Car car in cars)
                car.Draw(gameTime, spriteBatch);
        }

        public void AddCar(int numberOfCars = 1)
        {
            if (numberOfCars <= 0)
                return;

            for (int index = 0; index < numberOfCars; index++)
                cars.Add(GenerateCar());
        }

        public void AddSpawnPoint(Point spawnPoint)
        {
            spawnPoints.Add(spawnPoint);
        }

        public void AddSpawnPoints(List<Point> spawnPoints)
        {
            foreach (Point point in spawnPoints)
                this.spawnPoints.Add(point);
        }

        public void Clear()
        {
            cars.Clear();
        }
    }
}
