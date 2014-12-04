using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TheBlindMan
{
    public class Menu
    {
        private List<Icon> menuItems;
        int selectedIndex;

        Color normal = Color.White;
        Color hilite = Color.Yellow;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;
        GamePadState gamePadState;
        GamePadState oldGamePadState;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                if (selectedIndex < -1)
                    selectedIndex = -1;
                if (selectedIndex >= menuItems.Count)
                    selectedIndex = menuItems.Count - 1;
            }
        }

        public Menu(List<Icon> menuItems)
        {
            this.menuItems = menuItems;
        }

        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) && oldKeyboardState.IsKeyDown(theKey);
        }

        private bool CheckButton(Buttons theButton)
        {
            return gamePadState.IsButtonUp(theButton) && oldGamePadState.IsButtonDown(theButton);
        }

        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);

            if (CheckKey(Keys.Down) || CheckButton(Buttons.DPadDown))
            {
                selectedIndex++;
                if (selectedIndex == menuItems.Count)
                    selectedIndex = 0;
            }
            if (CheckKey(Keys.Up) || CheckButton(Buttons.DPadUp))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                    selectedIndex = menuItems.Count - 1;
            }

            oldKeyboardState = keyboardState;
            oldGamePadState = gamePadState;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Icon item in menuItems)
                item.Draw(gameTime, spriteBatch);
        }
    }
}
