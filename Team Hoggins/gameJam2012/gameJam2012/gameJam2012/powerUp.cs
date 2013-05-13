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
    class powerUp
    {
        public SpriteManager tex { get; set; }
        public Vector2 position { get; set; }
        public Rectangle colissionRec { get; set; }
        public TimeSpan delay;

        public void Draw(SpriteBatch spriteBatch)
        {
            tex.Position = position;
            tex.origin = new Vector2(tex.texWidth/2, tex.texHeight/2);
            tex.Draw(spriteBatch);
            colissionRec = new Rectangle((int)position.X, (int)position.Y, tex.texWidth, tex.texHeight);
        }

        public void update(GameTime gameTime)
        {
            delay -= gameTime.ElapsedGameTime;
            //if (delay.Seconds <= 0)
                
        }
    }
}
