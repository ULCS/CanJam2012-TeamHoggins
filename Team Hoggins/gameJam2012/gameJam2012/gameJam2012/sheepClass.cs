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
    class sheepClass
    {
        public SpriteManager sheepText { get; set; }
        public Vector2 position { get; set; }
        public Rectangle sheepRectangle { get; set; }
        public bool active { get; set; }
        public bool die { get; set; }
        public TimeSpan delay;

        public void Draw(SpriteBatch spriteBatch)
        {
            active = false;
            sheepText.Position = position;
            sheepText.Draw(spriteBatch);
            sheepRectangle = new Rectangle((int)position.X, (int)position.Y, sheepText.texWidth, sheepText.texHeight);
        }

        public void update(GameTime gameTime)
        {
            delay -= gameTime.ElapsedGameTime;
            if (delay.Seconds <= 0)
                active = true;
        }
    }
}