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

        public int MaxCars { get { return maxCarCount; } }

        public int CurrentCarCount() { return cars.Count(); }

        public Lane(Point startPosition, Point endPosition,
            int maxCarCount = 2, int safeSpawnDistance = 200)
        {
            cars = new List<Car>();
            carsToRemove = new List<Car>();
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.maxCarCount = maxCarCount;
            this.safeSpawnDistance = safeSpawnDistance;
        }

        public void AddCar(Car car)
        {
            if (IsOpen())
                cars.Add(car);
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
            foreach (Car car in carsToRemove)
                cars.Remove(car);
            carsToRemove.Clear();
        }

        public void Update(GameTime gameTime)
        {
            
            foreach (Car car in cars)
                car.Update(gameTime);
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
