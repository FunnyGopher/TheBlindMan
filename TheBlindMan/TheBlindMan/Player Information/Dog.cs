﻿using Microsoft.Xna.Framework;
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
    class Dog : Player
    {
        private Vector2 moveVector;
        private Vector2 velocity;

        private bool hasBarked;
        private bool hasGrowled;

        private AudioEmitter emitter;
        private SoundEffect barkSound;
        private SoundEffect growlSound;
        private SoundEffect wimperSound;

        private Icon barkIcon;
        private Icon growlIcon;

        public Vector2 Velocity
        {
            get { return velocity; }
        }

        public AudioEmitter Emitter
        {
            get { return emitter; }
        }

        public Dog(PlayerIndex index)
            : this(index, 0, 0) {}

        public Dog(PlayerIndex index, float x, float y)
            : base(index, x, y)
        {
            Speed = 20;
            Scale = 1.1f;
            moveVector = new Vector2();
            velocity = new Vector2(0, 0);

            hasBarked = false;
            hasGrowled = false;

            Bounds = new Rectangle((int)X, (int)Y + (int)(8 + Scale), (int)(10 + Scale), (int)(8 * Scale));

            emitter = new AudioEmitter();
        }

        public override void LoadContent(ContentManager content)
        {
            // Walking animations
            Animation walkingFront = new Animation(content.Load<Texture2D>(@"Images/Dog/front_sheet"),
               new Point(16, 26), new Point(0, 0), new Point(4, 1), 200);
            Players.Dog.AddAnimation("walkingFront", walkingFront);

            Animation walkingLeft = new Animation(content.Load<Texture2D>(@"Images/Dog/left_sheet"),
               new Point(36, 26), new Point(0, 0), new Point(8, 1), 200);
            Players.Dog.AddAnimation("walkingLeft", walkingLeft);

            Animation walkingRight = new Animation(content.Load<Texture2D>(@"Images/Dog/right_sheet"),
               new Point(36, 26), new Point(0, 0), new Point(8, 1), 200);
            Players.Dog.AddAnimation("walkingRight", walkingRight);

            Animation walkingBack = new Animation(content.Load<Texture2D>(@"Images/Dog/back_sheet"),
               new Point(16, 26), new Point(0, 0), new Point(4, 1), 200);
            Players.Dog.AddAnimation("walkingBack", walkingBack);

            // Standing animations
            Animation standingFront = new Animation(content.Load<Texture2D>(@"Images/Dog/front"),
               new Point(16, 26), new Point(0, 0), new Point(1, 1), 200);
            Players.Dog.AddAnimation("standingFront", standingFront);

            Animation standingLeft = new Animation(content.Load<Texture2D>(@"Images/Dog/left"),
               new Point(36, 26), new Point(0, 0), new Point(1, 1), 200);
            Players.Dog.AddAnimation("standingLeft", standingLeft);

            Animation standingRight = new Animation(content.Load<Texture2D>(@"Images/Dog/right"),
               new Point(36, 26), new Point(0, 0), new Point(1, 1), 200);
            Players.Dog.AddAnimation("standingRight", standingRight);

            Animation standingBack = new Animation(content.Load<Texture2D>(@"Images/Dog/back"),
               new Point(16, 26), new Point(0, 0), new Point(1, 1), 200);
            Players.Dog.AddAnimation("standingBack", standingBack);

            Players.Dog.CurrentAnimationName = "standingBack";
            Players.Dog.Direction = "Back";

            barkSound = content.Load<SoundEffect>(@"Audio/bark");
            growlSound = content.Load<SoundEffect>(@"Audio/growl");
            wimperSound = content.Load<SoundEffect>(@"Audio/dogYelp");

            barkIcon = new Icon(content.Load<Texture2D>(@"Images/Dog/bark"), false);
            growlIcon = new Icon(content.Load<Texture2D>(@"Images/Dog/growl"), false);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Console.WriteLine("X: " + X + ", Y: " + Y);

            if (!Alive)
                return;

            Move(gameTime);
            UpdateBounds();
            Actions();
            UpdateIcons();
        }

        public void UpdateBounds()
        {
            int boundsHeight = (int)(CurrentAnimation.FrameSize.Y * Scale * .3334f);
            Bounds = new Rectangle((int)X, (int)Y + (int)(CurrentAnimation.FrameSize.Y * Scale) - boundsHeight,
                (int)(CurrentAnimation.FrameSize.X * Scale), boundsHeight);
        }

        private void UpdateIcons()
        {
            barkIcon.X = (X + (Animations[CurrentAnimationName].FrameSize.X) / 2) - (barkIcon.Texture.Width / 2) + 3;
            barkIcon.Y = (Y - barkIcon.Texture.Height - 7);

            growlIcon.X = (X + (Animations[CurrentAnimationName].FrameSize.X) / 2) - (growlIcon.Texture.Width / 2) + 3;
            growlIcon.Y = (Y - growlIcon.Texture.Height - 7);
        }

        // Movement
        public override void Move(GameTime gameTime)
        {
            GamePadDPad dPad = GamePadState.DPad;
            KeyboardState keyboard = Keyboard.GetState();
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
                Bounds = new Rectangle((int)X, (int)Y + (int)(8 + Scale), (int)(10 + Scale), (int)(8 * Scale));
            }

            if (dPad.Down == ButtonState.Pressed)
            {
                velocity.Y = Speed;
                CurrentAnimationName = "walkingFront";
                Direction = "Front";
                Bounds = new Rectangle((int)X, (int)Y + (int)(8 + Scale), (int)(10 + Scale), (int)(8 * Scale));
            }

            if (dPad.Left == ButtonState.Pressed)
            {
                velocity.X = -Speed;
                CurrentAnimationName = "walkingLeft";
                Direction = "Left";
                Bounds = new Rectangle((int)X, (int)Y + (int)(8 + Scale), (int)(24 + Scale), (int)(8 * Scale));
            }

            if (dPad.Right == ButtonState.Pressed)
            {
                velocity.X = Speed;
                CurrentAnimationName = "walkingRight";
                Direction = "Right";
                Bounds = new Rectangle((int)X, (int)Y + (int)(8 + Scale), (int)(24 + Scale), (int)(8 * Scale));
            }

            //Arrow Keys
            if (keyboard.IsKeyDown(Keys.Up))
            {
                velocity.Y = -Speed;
                CurrentAnimationName = "walkingBack";
                Direction = "Back";
                Bounds = new Rectangle((int)X, (int)Y + (int)(8 + Scale), (int)(10 + Scale), (int)(8 * Scale));
            }

            if (keyboard.IsKeyDown(Keys.Down))
            {
                velocity.Y = Speed;
                CurrentAnimationName = "walkingFront";
                Direction = "Front";
                Bounds = new Rectangle((int)X, (int)Y + (int)(8 + Scale), (int)(10 + Scale), (int)(8 * Scale));
            }

            if (keyboard.IsKeyDown(Keys.Right))
            {
                velocity.X = Speed;
                CurrentAnimationName = "walkingRight";
                Direction = "Right";
                Bounds = new Rectangle((int)X, (int)Y + (int)(8 + Scale), (int)(24 + Scale), (int)(8 * Scale));
            }

            if (keyboard.IsKeyDown(Keys.Left))
            {
                velocity.X = -Speed;
                CurrentAnimationName = "walkingLeft";
                Direction = "Left";
                Bounds = new Rectangle((int)X, (int)Y + (int)(8 + Scale), (int)(24 + Scale), (int)(8 * Scale));
            }

            // Left Thumbstick 
            if (moveVector.Y > 0)
            {
                velocity.Y = -Speed;
                CurrentAnimationName = "walkingBack";
                Direction = "Back";
                Bounds = new Rectangle((int)X, (int)Y + (int)(8 + Scale), (int)(10 + Scale), (int)(8 * Scale));
            }

            if (moveVector.Y < 0)
            {
                velocity.Y = Speed;
                CurrentAnimationName = "walkingFront";
                Direction = "Front";
                Bounds = new Rectangle((int)X, (int)Y + (int)(8 + Scale), (int)(10 + Scale), (int)(8 * Scale));
            }

            if (moveVector.X > 0)
            {
                velocity.X = Speed;
                CurrentAnimationName = "walkingRight";
                Direction = "Right";
                Bounds = new Rectangle((int)X, (int)Y + (int)(8 + Scale), (int)(24 + Scale), (int)(8 * Scale));
            }

            if (moveVector.X < 0)
            {
                velocity.X = -Speed;
                CurrentAnimationName = "walkingLeft";
                Direction = "Left";
                Bounds = new Rectangle((int)X, (int)Y + (int)(8 + Scale), (int)(24 + Scale), (int)(8 * Scale));
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

            float height = Animations[CurrentAnimationName].FrameSize.Y;

            if (Y + height > 1430)
            {
                Y = 1430 - height;
                velocity.Y = 0;
            }

            X += velocity.X * (float)(gameTime.ElapsedGameTime.Milliseconds / 200f);
            Y += velocity.Y * (float)(gameTime.ElapsedGameTime.Milliseconds / 200f);

            Bounds = new Rectangle((int)X, (int)Y + 9, 26, 9);
            emitter.Position = new Vector3((X + (CurrentAnimation.FrameSize.X / 2)) / 8f, 0, (Y + (CurrentAnimation.FrameSize.X / 2)) / 8f);
        }

        // Actions
        private void Actions()
        {
            if (GamePadState.Buttons.A == ButtonState.Pressed && hasBarked == false)
            {
                Bark();
                hasBarked = true;
            }

            if (GamePadState.Buttons.B == ButtonState.Pressed && hasGrowled == false)
            {
                Growl();
                hasGrowled = true;
            }

            if (GamePadState.Buttons.A == ButtonState.Released)
            {
                barkIcon.Visible = false;
                hasBarked = false;
            }

            if (GamePadState.Buttons.B == ButtonState.Released)
            {
                growlIcon.Visible = false;
                hasGrowled = false;
            }
        }

        private void Bark()
        {
            Vector2 toOldMan = new Vector2(Players.OldMan.X - X, -1 * (Players.OldMan.Y - Y));
            float distanceToOldMan = toOldMan.Length();

            SoundEffectInstance bark = barkSound.CreateInstance();
            bark.Apply3D(Players.OldMan.AudioListener, emitter);
            bark.Play();

            barkIcon.Visible = true;

            Console.WriteLine("Bark!");
        }

        private void Growl()
        {
            Vector2 toOldMan = new Vector2(Players.OldMan.X - X, -1 * (Players.OldMan.Y - Y));
            float distanceToOldMan = toOldMan.Length();

            SoundEffectInstance growl = growlSound.CreateInstance();
            growl.Apply3D(Players.OldMan.AudioListener, emitter);
            growl.Play();

            growlIcon.Visible = true;

            Console.WriteLine("Grrrrrrrr!");
        }

        public override void Hit()
        {
            base.Hit();
            if (Alive)
            {
                Vector2 toOldMan = new Vector2(Players.OldMan.X - X, -1 * (Players.OldMan.Y - Y));
                float distanceToOldMan = toOldMan.Length();

                SoundEffectInstance wimper = wimperSound.CreateInstance();
                wimper.Apply3D(Players.OldMan.AudioListener, emitter);
                wimper.Play();

                Console.WriteLine("Dog has died!");
                CurrentAnimationName = "standingBack";
                X = 0;
                Y = 0;
                UpdateBounds();
                Alive = false;
                barkIcon.Visible = false;
                growlIcon.Visible = false;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            if (barkIcon.Visible)
            {
                spriteBatch.Draw(barkIcon.Texture, new Vector2(barkIcon.X, barkIcon.Y),
                    barkIcon.Texture.Bounds, Color.White, 0, new Vector2(0, 0),
                    1, SpriteEffects.None, 1f - ((Y + CurrentAnimation.FrameSize.Y) / 1440f));
            }

            if (growlIcon.Visible)
            {
                spriteBatch.Draw(growlIcon.Texture, new Vector2(growlIcon.X, growlIcon.Y),
                     growlIcon.Texture.Bounds, Color.White, 0, new Vector2(0, 0),
                     1, SpriteEffects.None, 1f - ((Y + CurrentAnimation.FrameSize.Y) / 1440f));
            }
        }
    }
}
