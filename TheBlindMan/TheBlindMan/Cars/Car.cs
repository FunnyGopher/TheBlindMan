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
        private bool animate;

        private Rectangle bounds;
        private int boundsHeight;

        private AudioEmitter emitter;
        private SoundEffect drivingSound;
        private SoundEffectInstance drivingSoundInstance;

        public float X
        {
            get { return this.x; }
            set { this.x = value; }
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

        public bool Animate
        {
            get { return this.animate; }
            set { this.animate = value; }
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

        public Car(Animation animation, int boundsHeight)
            : this(animation, 0, 0, 0, boundsHeight) {}

        public Car(Animation animation, float x, float y, float speed, int boundsHeight) 
        {
            this.x = x;
            this.y = y;
            this.speed = speed;
            this.animation = animation;
            this.animate = true;

            this.boundsHeight = boundsHeight;
            bounds = new Rectangle((int)X, (int)Y + animation.FrameSize.Y - boundsHeight, (int)animation.FrameSize.X, boundsHeight);

            emitter = new AudioEmitter();
        }

        public virtual void Update(GameTime gameTime)
        {
            if(speed > 0)
                animation.Update(gameTime);
            Move(gameTime);

            if(speed > 0)
                PlaySound();
            Collide();
            
        }

        private void Move(GameTime gameTime)
        {
            x += speed * (float)(gameTime.ElapsedGameTime.Milliseconds / 200f);
            emitter.Position = new Vector3(X / 8f, 0, Y / 8f);
            UpdateBounds();
        }

        private void UpdateBounds()
        {
            bounds.X = (int)X;
            bounds.Y = (int)Y + animation.FrameSize.Y - boundsHeight;
        }

        private void PlaySound()
        {
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
                if(speed > 0)
                {
                    Players.OldMan.Hit();
                }
                else
                {
                    float xDir = Players.OldMan.Velocity.X / Math.Abs(Players.OldMan.Velocity.X);
                    float yDir = Players.OldMan.Velocity.Y / Math.Abs(Players.OldMan.Velocity.Y);

                    Rectangle overlap = Rectangle.Intersect(bounds, Players.OldMan.Bounds);
                    if (overlap.Width < overlap.Height)
                        Players.OldMan.X += -xDir * overlap.Width;
                    else if(overlap.Height < overlap.Width)
                        Players.OldMan.Y += -yDir * overlap.Height;
                    else if(overlap.Height == overlap.Width)
                    {
                        Players.OldMan.X += -xDir * overlap.Width;
                        Players.OldMan.Y += -yDir * overlap.Height;
                    }
                }
            }

            if (bounds.Intersects(Players.Dog.Bounds))
            {
                if (speed > 0)
                {
                    Players.Dog.Hit();
                }
                else
                {
                    float xDir = Players.Dog.Velocity.X / Math.Abs(Players.Dog.Velocity.X);
                    float yDir = Players.Dog.Velocity.Y / Math.Abs(Players.Dog.Velocity.Y);

                    Rectangle overlap = Rectangle.Intersect(bounds, Players.Dog.Bounds);
                    if (overlap.Width < overlap.Height)
                        Players.Dog.X += -xDir * overlap.Width;
                    else if (overlap.Height < overlap.Width)
                        Players.Dog.Y += -yDir * overlap.Height;
                    else if (overlap.Height == overlap.Width)
                    {
                        Players.Dog.X += -xDir * overlap.Width;
                        Players.Dog.Y += -yDir * overlap.Height;
                    }
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

            spriteBatch.Draw(animation.TextureImage, new Vector2(x, y), 
                animation.GetCurrentFrameRectangle(), Color.White, 0, new Vector2(0, 0),
                1, effect, 1f - (y / 1440f));
        } 
    }
}
