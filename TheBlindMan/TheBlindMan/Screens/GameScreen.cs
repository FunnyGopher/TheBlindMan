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
    public abstract class GameScreen : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private List<GameComponent> components = new List<GameComponent>();
        private TheBlindManGame game;

        public List<GameComponent> Components
        {
            get { return components; }
        }

        public TheBlindManGame Game
        {
            get { return game; }
        }

        public GameScreen(TheBlindManGame game)
            : base(game)
        {
            this.game = game;
            Hide();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public virtual void LoadContent(ContentManager content)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (GameComponent component in components)
                if (component.Enabled == true)
                    component.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            foreach (GameComponent component in components)
                if (component is DrawableGameComponent &&
                    ((DrawableGameComponent)component).Visible)
                    ((DrawableGameComponent)component).Draw(gameTime);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Draw(gameTime);
        }

        public virtual void Start()
        {
            Show();
        }

        public virtual void Stop()
        {
            Hide();
        }

        public virtual void Show()
        {
            this.Visible = true;
            this.Enabled = true;
            foreach (GameComponent component in components)
            {
                component.Enabled = true;
                if (component is DrawableGameComponent)
                    ((DrawableGameComponent)component).Visible = true;
            }
        }

        public virtual void Hide()
        {
            this.Visible = false;
            this.Enabled = false;
            foreach (GameComponent component in components)
            {
                component.Enabled = false;
                if (component is DrawableGameComponent)
                    ((DrawableGameComponent)component).Visible = false;
            }
        }
    }
}
