using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheBlindMan
{
    class Camera2D : GameComponent, ICamera2D
    {
        private Vector2 position;
        private float viewportHeight;
        private float viewportWidth;

        public Camera2D(Game game)
            : base(game)
        {}

        public Vector2 Position
        {
            get { return position; }
            set { this.position = value; }
        }
        public float MoveSpeed
        {
            get;
            set;
        }
        public float Rotation
        {
            get;
            set;
        }
        public Vector2 Origin
        {
            get;
            set;
        }
        public float Scale
        {
            get;
            set;
        }
        public Vector2 ScreenCenter
        {
            get;
            protected set;
        }
        public Matrix Transform
        {
            get;
            set;
        }
        public IFocusable Focus
        {
            get;
            set;
        }

        /// <summary>
        /// Called when the GameComponent needs to be initialized. 
        /// </summary>
        public override void Initialize()
        {
            viewportWidth = Game.GraphicsDevice.Viewport.Width;
            viewportHeight = Game.GraphicsDevice.Viewport.Height;

            ScreenCenter = new Vector2(viewportWidth/2, viewportHeight/2);
            Scale = 1;
            MoveSpeed = 1.25f;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // Create the Transform used by any
            // spritebatch process
            Transform = Matrix.Identity*
                        Matrix.CreateTranslation(-Position.X, -Position.Y, 0)*
                        Matrix.CreateRotationZ(Rotation)*
                        Matrix.CreateTranslation(Origin.X, Origin.Y, 0)*
                        Matrix.CreateScale(new Vector3(Scale, Scale, Scale));

            Origin = ScreenCenter / Scale;

            // Move the Camera to the position that it needs to go
            var delta = (float) gameTime.ElapsedGameTime.TotalSeconds;

            position.X += (Focus.Position.X - Position.X) * MoveSpeed * delta;
            position.Y += (Focus.Position.Y - Position.Y) * MoveSpeed * delta;

            base.Update(gameTime);
        }

        public bool IsInView(Vector2 position, Texture2D texture)
        {
            if ( (position.X + texture.Width) < (Position.X - Origin.X) || (position.X) > (Position.X + Origin.X) )
                return false;

            if ((position.Y + texture.Height) < (Position.Y - Origin.Y) || (position.Y) > (Position.Y + Origin.Y))
                return false;

            return true;
        }
    }
}
