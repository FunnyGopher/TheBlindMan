using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBlindMan
{
    class Car
    {
        private float x, y;
        private float speed;
        private Animation animation;
        private bool parked;

        private Rectangle bounds;
        private int boundsHeight;

        private AudioEmitter emitter;
        private SoundEffect drivingSound;
        private SoundEffectInstance drivingSoundInstance;

        public float X
        {
            get { return this.x; }
            set
            { this.x = value; }
        }

        public float Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        public float Speed
        {
            get { return this.speed; }
            set { this.speed = value; }
        }

        public Animation Animation
        {
            get { return animation; }
        }

        public bool Parked
        {
            get { return this.parked; }
            set { this.parked = value; }
        }

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        public AudioEmitter Emitter
        {
            get { return emitter; }
        }

        public SoundEffect SoundEffect
        {
            get { return drivingSound; }
            set
            {
                drivingSound = value;
                drivingSoundInstance = drivingSound.CreateInstance();
                drivingSoundInstance.IsLooped = true;
            }
        }

        public Car(Car car)
            : this(car.animation, car.boundsHeight) {}

        public Car(Animation animation, int boundsHeight = 35)
            : this(animation, 0, 0, 0, boundsHeight) {}

        public Car(Animation animation, float x, float y, float speed, int boundsHeight) 
        {
            this.x = x;
            this.y = y;
            this.speed = speed;
            this.animation = animation;
            this.parked = false;

            this.boundsHeight = boundsHeight;
            bounds = new Rectangle((int)X, (int)Y + animation.FrameSize.Y - boundsHeight, (int)animation.FrameSize.X, boundsHeight);

            emitter = new AudioEmitter();
        }

        public virtual void Update(GameTime gameTime)
        {
            Animate(gameTime);
            Move(gameTime);
            UpdateBounds();
            PlaySound();
            Collide();
        }

        public void Park()
        {
            Animation.SheetSize = new Point(1, 1);
            parked = true;
        }

        private void Animate(GameTime gameTime)
        {
            if (parked)
                return;

            animation.Update(gameTime);
        }

        private void Move(GameTime gameTime)
        {
            if (parked)
                return;

            x += speed * (float)(gameTime.ElapsedGameTime.Milliseconds / 200f);
            emitter.Position = new Vector3(x / 8f, 0, y / 8f);
        }

        private void UpdateBounds()
        {
            bounds.X = (int)X;
            bounds.Y = (int)Y + animation.FrameSize.Y - boundsHeight;
        }

        private void PlaySound()
        {
            if (parked)
                return;

            Vector2 toOldMan = new Vector2(Players.OldMan.X - X, -1 * (Players.OldMan.Y - Y));
            float distanceToOldMan = toOldMan.Length();

            emitter.DopplerScale = distanceToOldMan * 2;
            emitter.Forward = new Vector3(speed, 0, 0);

            if (distanceToOldMan < 240)
            {
                drivingSoundInstance.Apply3D(Players.OldMan.AudioListener, emitter);
                drivingSoundInstance.Play();
            }
            else if (drivingSoundInstance.State == SoundState.Playing)
            {
                drivingSoundInstance.Stop();
            }
        }

        private void Collide()
        {
            if (bounds.Intersects(Players.OldMan.Bounds))
            {
                if(parked)
                {
                    float xTemp = Players.OldMan.Velocity.X == 0 ? 1 : Players.OldMan.Velocity.X;
                    float yTemp = Players.OldMan.Velocity.Y == 0 ? 1 : Players.OldMan.Velocity.Y;

                    float xDir = Players.OldMan.Velocity.X / Math.Abs(xTemp);
                    float yDir = Players.OldMan.Velocity.Y / Math.Abs(yTemp);

                    Console.WriteLine("Colliding!");

                    Rectangle overlap = Rectangle.Intersect(bounds, Players.OldMan.Bounds);

                    if (overlap.Width < overlap.Height)
                    {
                        if (Players.OldMan.X < X)
                            Players.OldMan.X = X - Players.OldMan.CurrentAnimation.FrameSize.X - 5;

                        if (Players.OldMan.X > X)
                            Players.OldMan.X = X + Animation.FrameSize.X + 5;
                    }
                    else if (overlap.Height < overlap.Width)
                        Players.OldMan.Y += -yDir * overlap.Height;
                    else if (overlap.Height == overlap.Width)
                    {
                        Players.OldMan.X += -xDir * overlap.Width;
                        Players.OldMan.Y += -yDir * overlap.Height;
                    }
                }
                else
                {
                    Players.OldMan.Hit();
                }
            }

            if (bounds.Intersects(Players.Dog.Bounds))
            {
                if (parked)
                {

                    float xTemp = Players.Dog.Velocity.X == 0 ? 1 : Players.Dog.Velocity.X;
                    float yTemp = Players.Dog.Velocity.Y == 0 ? 1 : Players.Dog.Velocity.Y;

                    float xDir = Players.Dog.Velocity.X / Math.Abs(xTemp);
                    float yDir = Players.Dog.Velocity.Y / Math.Abs(yTemp);

                    Rectangle overlap = Rectangle.Intersect(bounds, Players.Dog.Bounds);
                    if (overlap.Width < overlap.Height)
                    {
                        if (Players.Dog.X < X)
                            Players.Dog.X = X - Players.Dog.CurrentAnimation.FrameSize.X - 5;

                        if (Players.Dog.X > X)
                            Players.Dog.X = X + Animation.FrameSize.X + 5;
                    }
                    else if (overlap.Height < overlap.Width)
                        Players.Dog.Y += -yDir * overlap.Height;
                    else if (overlap.Height == overlap.Width)
                    {
                        Players.Dog.X += -xDir * overlap.Width;
                        Players.Dog.Y += -yDir * overlap.Height;
                    }
                }
                else
                {
                    Players.Dog.Hit();
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects effect;
            if (speed > 0)
                effect = SpriteEffects.None;
            else
                effect = SpriteEffects.FlipHorizontally;

            float bottomEdge = y + animation.FrameSize.Y;

            spriteBatch.Draw(animation.TextureImage, new Vector2(x, y), 
                animation.GetCurrentFrameRectangle(), Color.White, 0, new Vector2(0, 0),
                1, effect, 1f - (bottomEdge / 1440f));
        } 
    }
}
