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
        private Menu menu;
        private List<Icon> menuItems;

        private Texture2D backgroundImage;

        private bool hasPressedEnter;

        private int SelectedIndex
        {
            get { return menu.SelectedIndex; }
            set { menu.SelectedIndex = value; }
        }

        public StartScreen(TheBlindManGame game)
            : base(game)
        {
            menuItems = new List<Icon>();
            hasPressedEnter = false;
        }

        public override void LoadContent(ContentManager content)
        {
            int lineSpace = 20;
            menuItems.Add(new Icon(content.Load<Texture2D>(@"Images/Menu/Start"), true));
            menuItems.Add(new Icon(content.Load<Texture2D>(@"Images/Menu/Help"), true));
            menuItems.Add(new Icon(content.Load<Texture2D>(@"Images/Menu/Credits"), true));
            menuItems.Add(new Icon(content.Load<Texture2D>(@"Images/Menu/Quit"), true));

            backgroundImage = content.Load<Texture2D>(@"Images/Backgrounds/Title");

            for (int i = 0; i < menuItems.Count; i++)
            {
                Icon icon = menuItems[i];
                icon.X = (backgroundImage.Width / 2) - (icon.Texture.Width / 2);

                if (i == 0)
                    icon.Y = 275;
                else
                {
                    Icon prevIcon = menuItems[i - 1];
                    icon.Y = prevIcon.Y + prevIcon.Texture.Height + lineSpace;
                }
            }

            menu = new Menu(menuItems);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            menu.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) ||
                GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                hasPressedEnter = true;

            if (hasPressedEnter)
            {
                switch (SelectedIndex)
                {
                    case 0:
                        game.ActiveScreen = game.PlayScreen;
                        hasPressedEnter = false;
                        break;
                    case 1:
                        game.ActiveScreen = game.InfoScreen;
                        hasPressedEnter = false;
                        break;
                    case 2:
                        game.ActiveScreen = game.CreditScreen;
                        hasPressedEnter = false;
                        break;
                    case 3:
                        game.Exit();
                        break;
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Released)
                hasPressedEnter = false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundImage, backgroundImage.Bounds, Color.White);
            menu.Draw(gameTime, spriteBatch);
            base.Draw(gameTime, spriteBatch);
        }
    }
}
