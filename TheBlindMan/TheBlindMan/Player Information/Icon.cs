using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBlindMan
{
    public class Icon
    {
        private float x;
        private float y;
        private Texture2D texture;
        private bool visible;

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public Icon(Texture2D texture, bool visible)
        {
            x = 0;
            y = 0;
            this.texture = texture;
            this.visible = visible;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(x, y), Color.White);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(texture, new Vector2(x, y), color);
        }
    }
}
