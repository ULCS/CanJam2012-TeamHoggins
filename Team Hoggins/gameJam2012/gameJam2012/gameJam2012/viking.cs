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
    class viking
    {
        public Vector2 position { get; set; }  //  sprite position on screen
        public Vector2 velocity { get; set; }
        public int sheepBombs { get; set; }
        public int lives { get; set; }
        public Rectangle playerRectangle { get; set; }
        public TimeSpan vib_duration = new TimeSpan(0, 0, 0);
        public bool visible { get; set; }

        private Vector2 screenSize;
        private InputHandler input;
        public SpriteManager playerTex;
        private SpriteManager sheepTex;
        private proxSheepClass Sheep;
        private PlayerIndex controller;
        
        

        public List<proxSheepClass> proxList = new List<proxSheepClass>();
        public List<viking> players = new List<viking>();

        public viking(PlayerIndex x, Vector2 newPosition, GraphicsDeviceManager graphics, SpriteManager newPlayerTex, SpriteManager newSheepTex)
        {
            position = newPosition;
            sheepTex = newSheepTex;
            playerTex = newPlayerTex;
            controller = x;
            screenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            Sheep = new proxSheepClass(sheepTex, position, 0.5f);
            input = new InputHandler(x);
            sheepBombs = 5;
            lives = 2;
            visible = true;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            if (velocity.X < 0)
                playerTex.SetFrame(3);
            else
            {
                if (velocity.X > 0)
                    playerTex.SetFrame(2);
                else
                {
                    if (velocity.Y < 0)
                        playerTex.SetFrame(1);
                    else
                        playerTex.SetFrame(0);
                }
            }
            if (visible)
            {
                playerTex.Position = position;
                playerTex.Draw(spriteBatch);
            }

            for(int i = 0; i < proxList.Count; i++)
                proxList[i].Draw(spriteBatch);
        }

        public void player_update(GameTime gameTime)
        {
            if (lives > 4)
                lives = 4;
            if (sheepBombs > 10)
                sheepBombs = 10;

            if (vib_duration > new TimeSpan(0, 0, 0, 0 ,0))
            {
                GamePad.SetVibration(controller, 0.8f, 0.8f);
                vib_duration -= gameTime.ElapsedGameTime;
            }else
                GamePad.SetVibration(controller, 0f, 0f);

            input.InputUpdate();
            velocity = input.MovePlayer;
            for (int i = 0; i < proxList.Count; i++)
                proxList[i].update(gameTime);


            if (position.X + 52 + velocity.X > screenSize.X)
                velocity = new Vector2(-velocity.X, velocity.Y);
            if (position.Y + 52 + velocity.Y > screenSize.Y)
                velocity = new Vector2(velocity.X, -velocity.Y);
            if (position.X + velocity.X < 0)
                velocity = new Vector2(-velocity.X, velocity.Y);
            if (position.Y + velocity.Y < 0)
                velocity = new Vector2(velocity.X, -velocity.Y);

            if (visible)
                position += velocity;

            if (input.PlaceSheep && sheepBombs > 0)
            {
                proxList.Add(new proxSheepClass(sheepTex, new Vector2((position.X), (position.Y)), 2));
                sheepBombs--;
            }

        }

        public bool recCollision(Rectangle test)
        {
            if (this.playerRectangle.Intersects(test))
                return true;
            else
                return false;
        }
    }
}
    

