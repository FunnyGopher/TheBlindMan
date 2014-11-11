using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TheBlindMan
{
    public class InfoScreen : GameScreen
    {
        KeyboardState keyboardState;
        Texture2D image;
        Rectangle imageRectangle;

        public InfoScreen(TheBlindManGame game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
            imageRectangle = new Rectangle(
                0, 0, Game.Window.ClientBounds.Width,
                Game.Window.ClientBounds.Height);
        }

        protected override void LoadContent(ContentManager content)
        {
            image = content.Load<Texture2D>("Images/Backgrounds/Controls");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                game.Exit();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(image, imageRectangle, Color.White);
            base.Draw(gameTime);
        }
    }
}
