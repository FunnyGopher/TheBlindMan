using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace TheBlindMan
{
    class Player
    {
        private float x, y;
        private float speed;
        private Dictionary<string, Animation> animations;
        private bool alive;
        private string currentAnimationName;
        private string direction;
        private Rectangle bounds;
        private float scale;

        private PlayerIndex playerIndex;
        private GamePadState gamePadState;

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

        public bool Alive
        {
            get { return this.alive; }
            set { alive = value; }
        }

        public string CurrentAnimationName
        {
            get { return this.currentAnimationName; }
            set { this.currentAnimationName = value;}
        }

        public string Direction
        {
            get;
            set;
        }

        public PlayerIndex PlayerIndex
        {
            get { return playerIndex; }
        }

        public GamePadState GamePadState
        {
            get { return gamePadState; }
        }

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public Player(PlayerIndex index)
            :this(index, 0,0)
        {
        }

        public Player(PlayerIndex index, float x, float y) 
        {
            this.x = x;
            this.y = y;
            this.speed = 0;
            this.playerIndex = index;
            this.alive = true;
            this.animations = new Dictionary<string, Animation>();
            this.currentAnimationName = "";
            this.direction = "";
        }

        public virtual void LoadContent(ContentManager content)
        {

        }

        public void AddAnimation(string animationName, Animation animation) 
        {
            animations.Add(animationName, animation);
        }

        public virtual void Update(GameTime gameTime)
        {
            gamePadState = GamePad.GetState(playerIndex);

            if (animations.Keys.Contains(currentAnimationName))
            {
                Animation currentAnimation = animations[currentAnimationName];
                currentAnimation.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (animations.Keys.Contains(currentAnimationName))
            {
                Animation currentAnimation = animations[currentAnimationName];
                /*spriteBatch.Draw(currentAnimation.TextureImage, new Vector2(x,y),
                    currentAnimation.GetCurrentFrameRectangle(), Color.White);*/
                spriteBatch.Draw(currentAnimation.TextureImage, new Vector2(x, y),
                currentAnimation.GetCurrentFrameRectangle(), Color.White, 0, new Vector2(0, 0),
                scale, SpriteEffects.None, 1f - (y / 1440f));
            }
        }

        public virtual void Move(GameTime gameTime)
        {

        }

        public virtual void Hit()
        {

        }
    }
}
