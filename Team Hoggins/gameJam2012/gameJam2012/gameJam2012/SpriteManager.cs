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

namespace gameJam2012
{
    class SpriteManager
    {
        private Texture2D Texture;
        private Rectangle[] Rectangles;
        private int FrameIndex = 0;
        private bool flip = false;
        public bool active = false;
        public Vector2 origin = Vector2.Zero;

        public Vector2 Position = Vector2.Zero;
        public float Rotation = 0f;

        public Color[] textureData { get; set; }
        public int texWidth { get; set; }
        public int texHeight { get; set; }
        public float Scale { get; set; }

        public SpriteManager(Texture2D Texture, int frames, float scale)
        {
            Scale = scale;
            this.Texture = Texture;
            texWidth = Texture.Width / frames;
            texHeight = Texture.Height;
            Rectangles = new Rectangle[frames];
            for (int i = 0; i < frames; i++)
                Rectangles[i] = new Rectangle(i * texWidth, 0, texWidth, texHeight);

            this.textureData = new Color[Texture.Width * texHeight];
            this.Texture.GetData(this.textureData);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Rectangles[FrameIndex], Color.White, Rotation, origin, Scale, SpriteEffects.None, 0f);
        }

        public void SetFrame(int frame)
        {
            if (frame < Rectangles.Length)
                FrameIndex = frame;
        }

        public void incBounceFrame()
        {
            if (active)
            {
                if (FrameIndex < Rectangles.Length - 1 && flip == false)
                    FrameIndex++;
                else
                    flip = true;

                if (FrameIndex > 0 && flip == true)
                    FrameIndex--;
                else
                    flip = false;

                if (FrameIndex == 0)
                    active = false;
            }
        }
    }
}
