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

            this.boundsHeight = boundsHeight;
            bounds = new Rectangle((int)X, (int)Y + animation.FrameSize.Y - boundsHeight, (int)animation.FrameSize.X, boundsHeight);

            emitter = new AudioEmitter();
        }

        public virtual void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
            Move(gameTime);
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
                Players.OldMan.Hit();
            }

            if (bounds.Intersects(Players.Dog.Bounds))
            {
                Players.Dog.Hit();
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
                1, effect, .5f);
        } 
    }
}
