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
    class CarManager
    {
        private TheBlindManGame game;
        private List<Lane> lane;
        private List<Car> carsToRemove;
        private List<Point> spawnPoints;
        private Random random = new Random();
        private const int TOTAL_NUMBER_OF_CARS = 10;
        private const int TOTAL_TYPES_OF_CARS = 1;
        private List<SoundEffect> soundEffects;
        private Car[] preFabCars;
        //public int Count {get{return cars.Count;}}

        public CarManager(TheBlindManGame game)
        {
            this.game = game;

            //cars = new List<Car>();
            carsToRemove = new List<Car>();
            spawnPoints = new List<Point>();
            soundEffects = new List<SoundEffect>();
            preFabCars = new Car[TOTAL_TYPES_OF_CARS];     
        }

        public Car GenerateCar()
        {
            Car car = new Car(preFabCars[random.Next(0, TOTAL_TYPES_OF_CARS)]);

            Point spawnPoint = spawnPoints[random.Next(0, 8)]; 
            car.X = spawnPoint.X;
            car.Y = spawnPoint.Y;
            float speed = (float)random.Next(10, 30);
            speed *= spawnPoint.X <= 0 ? 1 : -1;
            car.Speed = speed;

            SoundEffect soundEffect = soundEffects[random.Next(0, soundEffects.Count)];
            car.SoundEffect = soundEffect;

            return car;
        }

        private Point getGoodSpawnPoint()
        {
            bool goodSpawnFound = false;
            while (!(goodSpawnFound))
            {
                Point SpawnPoint = spawnPoints[random.Next(0, 8)];
                //foreach (Car car in cars)
                //{
                //}
            }
            return new Point(1, 1);
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
            /*
            if (cars.Count < TOTAL_NUMBER_OF_CARS)
                AddCar();

            foreach (Car car in cars)
            {
                car.Update(gameTime);
                if (car.X + car.Bounds.Width < 0 - car.Bounds.Width || car.X > game.GraphicsDevice.Viewport.Width + car.Bounds.Width)
                    carsToRemove.Add(car);
            }

            foreach (Car car in carsToRemove)
                cars.Remove(car);

            carsToRemove.Clear();
             */
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //foreach (Lane lane in lanes)
                //lane.Draw(gameTime, spriteBatch);
        }

        public void AddCar()
        {
            //cars.Add(GenerateCar());
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
            //cars.Clear();
        }
    }
}
