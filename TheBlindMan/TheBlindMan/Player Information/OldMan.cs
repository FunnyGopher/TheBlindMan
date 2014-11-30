using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBlindMan
{
    class OldMan : Player
    {
        private const double MIN_PERCENT_FOR_LEFT_VIB = .8d;
        private const double RIGHT_VIB_PENALTY = .02d;
        private const int MAX_DOG_DISTANCE = 64;

        private Vector2 feelVector;
        private double leftVibPercentage;
        private double rightVibPercentage;

        private Vector2 moveVector;
        private Vector2 velocity;

        private bool hasAlerted;

        private AudioListener audioListener;
        private SoundEffect dyingSound;
        private SoundEffect whistleSound;

        public AudioListener AudioListener
        {
            get { return audioListener; }
        }

        public OldMan(PlayerIndex index) 
            : this(index, 0, 0) {}

        public OldMan(PlayerIndex index, float x, float y)
            : base(index, x, y)
        {
            Speed = 70;
            Scale = 1;
            feelVector = new Vector2();
            moveVector = new Vector2();
            velocity = new Vector2(0, 0);
            hasAlerted = false;

            Bounds = new Rectangle((int)X, (int)Y + (int)(16 * Scale), (int)(14 * Scale), (int)(16 * Scale));

            audioListener = new AudioListener();
        }

        public override void LoadContent(ContentManager content)
        {
            // Walking animations
            Animation walkingFront = new Animation(content.Load<Texture2D>(@"Images/OldMan/front_sheet"),
               new Point(28, 54), new Point(0, 0), new Point(8, 1), 125);
            Players.OldMan.AddAnimation("walkingFront", walkingFront);

            Animation walkingLeft = new Animation(content.Load<Texture2D>(@"Images/OldMan/left_sheet"),
               new Point(21, 54), new Point(0, 0), new Point(6, 1), 200);
            Players.OldMan.AddAnimation("walkingLeft", walkingLeft);

            Animation walkingRight = new Animation(content.Load<Texture2D>(@"Images/OldMan/right_sheet"),
               new Point(21, 54), new Point(0, 0), new Point(6, 1), 200);
            Players.OldMan.AddAnimation("walkingRight", walkingRight);

            Animation walkingBack = new Animation(content.Load<Texture2D>(@"Images/OldMan/back_sheet"),
               new Point(28, 54), new Point(0, 0), new Point(8, 1), 125);
            Players.OldMan.AddAnimation("walkingBack", walkingBack);

            // Standing animations
            Animation standingFront = new Animation(content.Load<Texture2D>(@"Images/OldMan/front"),
               new Point(28, 54), new Point(0, 0), new Point(1, 1), 200);
            Players.OldMan.AddAnimation("standingFront", standingFront);

            Animation standingLeft = new Animation(content.Load<Texture2D>(@"Images/OldMan/left"),
               new Point(21, 54), new Point(0, 0), new Point(1, 1), 200);
            Players.OldMan.AddAnimation("standingLeft", standingLeft);

            Animation standingRight = new Animation(content.Load<Texture2D>(@"Images/OldMan/right"),
               new Point(21, 54), new Point(0, 0), new Point(1, 1), 200);
            Players.OldMan.AddAnimation("standingRight", standingRight);

            Animation standingBack = new Animation(content.Load<Texture2D>(@"Images/OldMan/back"),
               new Point(28, 54), new Point(0, 0), new Point(1, 1), 200);
            Players.OldMan.AddAnimation("standingBack", standingBack);

            Players.OldMan.CurrentAnimationName = "standingBack";
            Players.OldMan.Direction = "Back";

            dyingSound = content.Load<SoundEffect>(@"Audio/manDying");
            whistleSound = content.Load<SoundEffect>(@"Audio/whistle");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!Alive)
            {
                return;
            }

            FeelingMechanic();
            Move(gameTime);
            Actions();
        }

        // Movement
        public override void Move(GameTime gameTime)
        {
            GamePadDPad dPad = GamePadState.DPad;
            KeyboardState keyState = Keyboard.GetState();
            moveVector = GamePadState.ThumbSticks.Left;

            velocity.X = 0;
            velocity.Y = 0;

            CurrentAnimationName = "standing" + Direction;

            // DPad
            if (dPad.Up == ButtonState.Pressed)
            {
                velocity.Y = -Speed;
                CurrentAnimationName = "walkingBack";
                Direction = "Back";
            }

            if (dPad.Down == ButtonState.Pressed)
            {
                velocity.Y = Speed;
                CurrentAnimationName = "walkingFront";
                Direction = "Front";
            }

            if (dPad.Left == ButtonState.Pressed)
            {
                velocity.X = -Speed;
                CurrentAnimationName = "walkingLeft";
                Direction = "Left";
            }

            if (dPad.Right == ButtonState.Pressed)
            {
                velocity.X = Speed;
                CurrentAnimationName = "walkingRight";
                Direction = "Right";
            }

            //WASD
            if (keyState.IsKeyDown(Keys.W))
            {
                velocity.Y = -Speed;
                CurrentAnimationName = "walkingBack";
                Direction = "Back";
            }

            if (keyState.IsKeyDown(Keys.S))
            {
                velocity.Y = Speed;
                CurrentAnimationName = "walkingFront";
                Direction = "Front";
            }

            if (keyState.IsKeyDown(Keys.D))
            {
                velocity.X = Speed;
                CurrentAnimationName = "walkingRight";
                Direction = "Right";
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                velocity.X = -Speed;
                CurrentAnimationName = "walkingLeft";
                Direction = "Left";
            }

            // Left Thumbstick
            if (moveVector.Y > 0)
            {
                velocity.Y = -Speed;
                CurrentAnimationName = "walkingBack";
                Direction = "Back";
            }

            if (moveVector.Y < 0)
            {
                velocity.Y = Speed;
                CurrentAnimationName = "walkingFront";
                Direction = "Front";
            }

            if (moveVector.X > 0)
            {
                velocity.X = Speed;
                CurrentAnimationName = "walkingRight";
                Direction = "Right";
            }

            if (moveVector.X < 0)
            {
                velocity.X = -Speed;
                CurrentAnimationName = "walkingLeft";
                Direction = "Left";
            }

            if (X < 0)
            {
                X = 0;
                velocity.X = 0;
            }

            if (X + Bounds.Width > 1080)
            {
                X = 1080 - Bounds.Width;
                velocity.X = 0;
            }

            if (Y < 0)
            {
                Y = 0;
                velocity.Y = 0;
            }

            if (Y + Bounds.Height > 1440)
            {
                Y = 1440 - Bounds.Height;
                velocity.Y = 0;
            }

            X += velocity.X * (float) (gameTime.ElapsedGameTime.Milliseconds / 200f);
            Y += velocity.Y * (float) (gameTime.ElapsedGameTime.Milliseconds / 200f);

            Bounds = new Rectangle((int)X, (int)Y + (int)(16 * Scale), (int)(14 * Scale), (int)(16 * Scale));
            audioListener.Position = new Vector3(X / 8f, 0, Y / 8f);
        }

        // Feeling Mechanic
        private void FeelingMechanic()
        {
            if (!Players.Dog.Alive)
            {
                return;
            }

            Vector2 toDog = new Vector2((Players.Dog.X + (Players.Dog.Bounds.Width / 2)) - (X + (Bounds.Width / 2)), -1 * ((Players.Dog.Y + (Players.Dog.Bounds.Height / 2)) - (Y + (Bounds.Height / 2))));
            float distanceToDog = toDog.Length();
            toDog.Normalize();

            feelVector = GamePadState.ThumbSticks.Right;
            feelVector.Normalize();
            leftVibPercentage = 0;
            rightVibPercentage = 0;

            if (feelVector.Length() > .75f)
            {
                double toDogDirection = GetDirection(toDog);
                double feelDirection = GetDirection(feelVector);

                double c = Math.Sqrt((toDog.X - feelVector.X) * (toDog.X - feelVector.X) + (toDog.Y - feelVector.Y) * (toDog.Y - feelVector.Y));
                double numerator = toDog.LengthSquared() + feelVector.LengthSquared() - (c * c);
                double denominator = 2f * toDog.Length() * feelVector.Length();
                double value = numerator / denominator;
                double diffRadians = Math.Acos(Math.Round(value, 6));
                double diffDegrees = diffRadians * (360d / ( 2d * Math.PI));

                if (diffDegrees < 90d)
                {
                    rightVibPercentage = 1 - (diffDegrees / 90d);
                }
            }

            if (rightVibPercentage > MIN_PERCENT_FOR_LEFT_VIB && distanceToDog <= MAX_DOG_DISTANCE * Scale)
            {
                leftVibPercentage = rightVibPercentage;
            }
            else if (distanceToDog > MAX_DOG_DISTANCE * Scale)
            {
                double outerDist = distanceToDog - MAX_DOG_DISTANCE * Scale;
                leftVibPercentage = 0d;
                rightVibPercentage -= outerDist * RIGHT_VIB_PENALTY;

                if (rightVibPercentage < 0)
                {
                    rightVibPercentage = 0d;
                }
            }

            GamePad.SetVibration(PlayerIndex, (float)leftVibPercentage * 1f, (float)rightVibPercentage * 1f);
        }

        // Actions
        private void Actions()
        {
            if (GamePadState.Buttons.RightStick == ButtonState.Pressed && hasAlerted == false)
            {
                Alert();
                hasAlerted = true;
            }

            if (GamePadState.Buttons.RightStick == ButtonState.Released)
            {
                hasAlerted = false;
            }
        }

        private void Alert()
        {
            SoundEffectInstance whistle = whistleSound.CreateInstance();
            whistle.Volume = .25f;
            whistle.Play();
            Console.WriteLine("*Whistle* Where's my dog!!!");
        }

        public override void Hit()
        {
            if (Alive)
            {
                SoundEffectInstance dying = dyingSound.CreateInstance();
                dying.Volume = .05f;
                dying.Play();

                Console.WriteLine("The old man has died!");
                CurrentAnimationName = "standingFront";
                //Alive = false;
                X = 540;
                Y = 900;
                Players.Dog.X = 570;
                Players.Dog.Y = 920;
            }
        }

        private double GetDirection(Vector2 vector)
        {
            double degrees = 0;
            double radians = Math.Asin(vector.Y / vector.Length());
            if (radians < 0)
            {
                radians = 2 * Math.PI + radians;
            }

            degrees = radians * (360d / (2d * Math.PI));
            return degrees;
        }
    }
}
