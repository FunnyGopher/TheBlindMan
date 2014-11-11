using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheBlindMan
{
    public class StartScreen : GameScreen
    {
        private TheBlindManGame game;
        private MenuComponent menuComponent;
        private string[] menuItems;

        private Texture2D image;
        private Rectangle imageRectangle;

        private bool hasPressedEnter;

        private int SelectedIndex
        {
            get { return menuComponent.SelectedIndex; }
            set { menuComponent.SelectedIndex = value; }
        }

        public StartScreen(TheBlindManGame game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
            this.game = game;
            menuItems = new string[]{ "Start Game", "Controls", "Credits", "End Game" };
            imageRectangle = new Rectangle(
                0, 0, Game.Window.ClientBounds.Width,
                Game.Window.ClientBounds.Height);
            hasPressedEnter = false;
        }

        public override void LoadContent(ContentManager content)
        {
            SpriteFont font = content.Load<SpriteFont>(@"Font/menuFont");
            menuComponent = new MenuComponent(game, spriteBatch, font, menuItems);
            Components.Add(menuComponent);

            image = content.Load<Texture2D>(@"Images/Backgrounds/Title");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                hasPressedEnter = true;

            if (hasPressedEnter)
            {
                switch (SelectedIndex)
                {
                    case 0:
                        game.ActiveScreen = game.PlayScreen;
                        break;
                    case 1:
                        game.ActiveScreen = game.InfoScreen;
                        break;
                    case 2:
                        game.ActiveScreen = game.CreditScreen;
                        break;
                    case 3:
                        game.Exit();
                        break;
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Released)
                hasPressedEnter = false;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(image, imageRectangle, Color.White);
            base.Draw(gameTime);
        }
    }
}
