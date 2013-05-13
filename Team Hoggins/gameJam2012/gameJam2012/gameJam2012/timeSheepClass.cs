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
    class timeSheepClass : sheepClass
    {
        public TimeSpan timeSpan = new TimeSpan(0, 0, 5);

        public timeSheepClass(SpriteManager newTexture, Vector2 newPosition)
        {
            sheepText = newTexture;
            position = newPosition;
        }

        public void Update(GameTime gameTime)
        {
            timeSpan -= gameTime.ElapsedGameTime;
        }
    }
}
