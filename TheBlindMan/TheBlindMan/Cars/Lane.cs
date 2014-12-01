using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace TheBlindMan
{
    class Lane
    {
        private List<Car> cars;
        private List<Car> carsToRemove;
        private Point startPosition;
        private Point endPosition;
        private int maxCarCount;
        private int safeSpawnDistance;

        private int direction;
        private int speed;

        private Random random = new Random();
        private int speedMod;

        public int MaxCars { get { return maxCarCount; } }

        public int CurrentCarCount() { return cars.Count(); }

        public Lane(Point startPosition, Point endPosition, int speed,
            int safeSpawnDistance = 0, int speedMod = 1, int maxCarCount = 2)
        {
            cars = new List<Car>();
            carsToRemove = new List<Car>();
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.maxCarCount = maxCarCount;
            this.safeSpawnDistance = 300 + safeSpawnDistance;

            this.direction = endPosition.X - startPosition.X;
            this.speed = speed;
            this.speedMod = speedMod;
        }

        public void AddCar(Car car)
        {
            if (IsOpen())
            {
                car.X = startPosition.X;
                car.Y = startPosition.Y;

                car.Speed = speed;
                if(direction < 0)
                    car.Speed *= -1;

                int dir = random.Next(0, 2);
                int mod = random.Next(0, speedMod + 1);

                if (dir == 1)
                    mod *= -1;
                car.Speed += mod;

                cars.Add(car);
            }
        }

        public bool IsOpen()
        {
            if (CurrentCarCount() != MaxCars && IsSafeZoneClear())
                return true;
            else
                return false;
        }

        private bool IsSafeZoneClear()
        {
            if (CurrentCarCount() == 0)
                return true;
            foreach (Car car in cars)
                if ((Math.Abs(car.X - startPosition.X) <= safeSpawnDistance))
                    return false;
            return true;
        }

        private void RemoveCars()
        {
            foreach (Car car in cars)
                if (car.X > endPosition.X && car.X > startPosition.X || 
                    car.X < endPosition.X && car.X < startPosition.X)
                    carsToRemove.Add(car);
        }

        public void Update(GameTime gameTime)
        {
            foreach (Car car in cars)
                car.Update(gameTime);

            RemoveCars();

            foreach (Car car in carsToRemove)
                cars.Remove(car);
            carsToRemove.Clear();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Car car in cars)
                car.Draw(gameTime, spriteBatch);
        }

        public void Clear()
        {
            foreach (Car car in cars)
                carsToRemove.Add(car);
            cars.Clear();
        }
    }
}
