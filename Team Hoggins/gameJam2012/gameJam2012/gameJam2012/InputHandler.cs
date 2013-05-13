using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework; // For most Microsoft functions, including PlayerIndex
using Microsoft.Xna.Framework.Input; // For the controller/keyboard input 

namespace gameJam2012
{
    class InputHandler
    {
        private PlayerIndex playerNo;
        public Vector2 MovePlayer { get; set; }
        public bool PlaceSheep { get; set; }
        public bool ready { get; set; }
        public GamePadState currentState { get; set; }
        public GamePadState previousState { get; set; }

        public InputHandler(PlayerIndex x) { 
            playerNo = x;
            currentState = GamePad.GetState(playerNo);
            previousState = GamePad.GetState(playerNo);
            ready = true;
        }

        public void InputUpdate()
        {
            MovePlayer = new Vector2(GamePad.GetState(playerNo).ThumbSticks.Left.X * 6, -GamePad.GetState(playerNo).ThumbSticks.Left.Y * 6);

            if (GamePad.GetState(playerNo).Buttons.A == ButtonState.Pressed && ready == true)
            {
                PlaceSheep = true;
                ready = false;
            }
            else
                PlaceSheep = false;

            if (GamePad.GetState(playerNo).Buttons.A == ButtonState.Released && ready == false)
                ready = true;
        }


    }
}
