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
    public class CreditScreen : GameScreen
    {
        private Texture2D image;
        private Rectangle imageRectangle;

        public CreditScreen(TheBlindManGame game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
            imageRectangle = new Rectangle(
                0, 0, Game.Window.ClientBounds.Width,
                Game.Window.ClientBounds.Height);
        }

        public override void LoadContent(ContentManager content)
        {
            image = content.Load<Texture2D>(@"Images/Backgrounds/Credits");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) ||
                GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
                game.ActiveScreen = game.StartScreen;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(image, imageRectangle, Color.White);
            base.Draw(gameTime);
        }
    }
}
