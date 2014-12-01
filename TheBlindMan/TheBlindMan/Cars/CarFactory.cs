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
        private const int TOTAL_TYPES_OF_CARS = 6;
        private const int MAX_NUMBER_OF_CARS = 12;
        private List<SoundEffect> soundEffects;
        private Car[] preFabCars;
        private Car[] parkedFabCars;

        public CarFactory()
        {
            lanes = new List<Lane>();
            soundEffects = new List<SoundEffect>();
            preFabCars = new Car[TOTAL_TYPES_OF_CARS];
            parkedFabCars = new Car[TOTAL_TYPES_OF_CARS];  
        }

        public Car GenerateCar()
        {
            Car car = new Car(preFabCars[random.Next(0, TOTAL_TYPES_OF_CARS)]);
            SoundEffect soundEffect = soundEffects[random.Next(0, soundEffects.Count)];
            car.SoundEffect = soundEffect;
            return car;
        }

        public Car GenerateParkedCar(float x, float y)
        {
            Car parkedCar = new Car(parkedFabCars[random.Next(0, TOTAL_TYPES_OF_CARS)]); ;
            parkedCar.Park();
            parkedCar.X = x;
            parkedCar.Y = y - parkedCar.Animation.FrameSize.Y;

            int direction = random.Next(0, 2);
            if (direction == 0)
                parkedCar.Speed = -1;
            else
                parkedCar.Speed = 1;

            return parkedCar;
        }

        public virtual void LoadContent(ContentManager content)
        {
            Animation elCaminoAnim = new Animation(content.Load<Texture2D>(@"Images/Cars/el_camino"),
               new Point(228, 65), new Point(0, 0), new Point(2, 1), 5000);
            Animation elCaminoStill = new Animation(content.Load<Texture2D>(@"Images/Cars/el_camino"),
               new Point(228, 65), new Point(0, 0), new Point(1, 1), 5000);
            preFabCars[0] = new Car(elCaminoAnim);
            parkedFabCars[0] = new Car(elCaminoStill);            

            Animation grayCarAnim = new Animation(content.Load<Texture2D>(@"Images/Cars/gray_car"),
               new Point(204, 75), new Point(0, 0), new Point(2, 1), 5000);
            Animation grayCarStill = new Animation(content.Load<Texture2D>(@"Images/Cars/gray_car"),
               new Point(204, 75), new Point(0, 0), new Point(1, 1), 5000);
            preFabCars[1] = new Car(grayCarAnim);
            parkedFabCars[1] = new Car(grayCarStill);

            Animation hummerAnim = new Animation(content.Load<Texture2D>(@"Images/Cars/hummer"),
               new Point(207, 93), new Point(0, 0), new Point(2, 1), 7000);
            Animation hummerStill = new Animation(content.Load<Texture2D>(@"Images/Cars/hummer"),
               new Point(207, 93), new Point(0, 0), new Point(1, 1), 7000);
            preFabCars[2] = new Car(hummerAnim);
            parkedFabCars[2] = new Car(hummerStill);

            Animation pickupAnim = new Animation(content.Load<Texture2D>(@"Images/Cars/pickup_truck"),
               new Point(225, 92), new Point(0, 0), new Point(2, 1), 5000);
            Animation pickupStill = new Animation(content.Load<Texture2D>(@"Images/Cars/pickup_truck"),
               new Point(225, 92), new Point(0, 0), new Point(1, 1), 5000); 
            preFabCars[3] = new Car(pickupAnim);
            parkedFabCars[3] = new Car(pickupStill);

            Animation redCarAnim = new Animation(content.Load<Texture2D>(@"Images/Cars/red_car"),
               new Point(185, 68), new Point(0, 0), new Point(2, 1), 5000);
            Animation redCarStill = new Animation(content.Load<Texture2D>(@"Images/Cars/red_car"),
               new Point(185, 68), new Point(0, 0), new Point(1, 1), 5000);
            preFabCars[4] = new Car(redCarAnim);
            parkedFabCars[4] = new Car(redCarStill);

            Animation smartCarAnim = new Animation(content.Load<Texture2D>(@"Images/Cars/smart_car"),
               new Point(86, 60), new Point(0, 0), new Point(2, 1), 2000);
            Animation smartCarStill = new Animation(content.Load<Texture2D>(@"Images/Cars/smart_car"),
               new Point(86, 60), new Point(0, 0), new Point(1, 1), 2000);
            preFabCars[5] = new Car(smartCarAnim);
            parkedFabCars[5] = new Car(smartCarStill);

            soundEffects.Add(content.Load<SoundEffect>(@"Audio/car"));
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
