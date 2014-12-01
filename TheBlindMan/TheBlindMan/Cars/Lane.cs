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
        private Point startPosition;
        private Point endPosition;
        private int maxCars;

        public int MaxCars { get { return maxCars; } }

        public Lane(Point startPosition, Point endPosition) { }

        public void AddCar(Car car)
        {
            cars.Add(car);
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
    }
}
