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
    class proxSheepClass : sheepClass
    {
        //proximityMine 

        public proxSheepClass(SpriteManager newTexture, Vector2 newPosition, float delay_time)
        {
            sheepText = newTexture;
            position = newPosition;
            delay = new TimeSpan(0,0,(int)delay_time);
        }
    }
}
