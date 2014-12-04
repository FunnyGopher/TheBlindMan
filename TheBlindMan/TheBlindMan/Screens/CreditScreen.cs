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
        private Texture2D backgroundImage;

        public CreditScreen(TheBlindManGame game)
            : base(game)
        {
        }

        public override void LoadContent(ContentManager content)
        {
            backgroundImage = content.Load<Texture2D>(@"Images/Backgrounds/Credits");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) ||
                GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
                Game.ActiveScreen = Game.StartScreen;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundImage, backgroundImage.Bounds, Color.White);
            base.Draw(gameTime, spriteBatch);
        }
    }
}
