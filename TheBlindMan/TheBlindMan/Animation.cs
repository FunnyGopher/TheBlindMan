#region File Information
/*
 * 
 * 
 */
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TheBlindMan
{
    class Animation
    {
        private Texture2D textureImage;
        private Point frameSize;
        private Point currentFrame;
        private Point sheetSize;
        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame;
        private const int DEFAULT_MILLISECONDS_PER_FRAME = 32;

        public Point FrameSize
        {
            get { return frameSize; }
        }

        public Animation(Texture2D textureImage, Point framesize, Point currentFrame, Point sheetSize)
            : this(textureImage, framesize, currentFrame, sheetSize, DEFAULT_MILLISECONDS_PER_FRAME)
        {
        }

        public Animation(Texture2D textureImage, Point framesize, Point currentFrame, Point sheetSize, int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.frameSize = framesize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        public virtual void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                    {
                        currentFrame.Y = 0;
                    }
                }
            }
        }

        public Rectangle GetCurrentFrameRectangle()
        {
            Rectangle currentFrameRectangle = 
                new Rectangle(currentFrame.X * frameSize.X,
                currentFrame.Y * frameSize.Y,
                frameSize.X, frameSize.Y);
            return currentFrameRectangle;
        }

        public Texture2D TextureImage
        {
            get
            {
                return this.textureImage;
            }
        }

        public Point CurrentFrame
        {
            get
            {
                return this.currentFrame;
            }
            set
            {
                currentFrame = value;
                timeSinceLastFrame = 0;
            }
        }
    }
}
