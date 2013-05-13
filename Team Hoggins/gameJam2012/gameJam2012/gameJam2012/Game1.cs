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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D backTile;
        Texture2D statue;
        Texture2D heart;

        SpriteManager sheepDeath;
        SpriteManager mask;
        Random rand = new Random();
        SpriteFont HUDfont;
        int[] deadPLayers;
        
        TimeSpan timeSpan = new TimeSpan(0, 0, 3, 21,2); //Denotes the time (3mins 21 secs)
        TimeSpan timeSpanLight = new TimeSpan(0, 0, 3);

        Vector2 timeLoc = new Vector2(450f, 35f); //The location data for the timer

        List<viking> players = new List<viking>();
        List<Vector2> playerHUD = new List<Vector2>();
        List<healthPowerUp> health = new List<healthPowerUp>();
        List<BombPowerUp> bombUp = new List<BombPowerUp>();


        private bool paused = true;
        private bool pauseKeyDown = false;
        private KeyboardState keyboardState;
        private GamePadState gamePadState;

        int sheepType;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 1024;
            graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            HUDfont = Content.Load<SpriteFont>("SpriteFont1");
            backTile = Content.Load<Texture2D>("grass");
            statue = Content.Load<Texture2D>("statue");
            mask = new SpriteManager(Content.Load<Texture2D>("softMaskStrip2"), 12, 8);
            heart = Content.Load<Texture2D>("8bitheartlogo");
            sheepType = 0;

            deadPLayers = new int[4] { 0, 0, 0, 0 }; 

            playerHUD.Add(new Vector2(50, 50));
            playerHUD.Add(new Vector2(graphics.PreferredBackBufferWidth - 150, 50));
            playerHUD.Add(new Vector2(50, graphics.PreferredBackBufferHeight-150));
            playerHUD.Add(new Vector2(graphics.PreferredBackBufferWidth - 150, graphics.PreferredBackBufferHeight-150));

            players.Add(new viking(PlayerIndex.One, new Vector2(100, 100), graphics, new SpriteManager(Content.Load<Texture2D>("vikingBlueStrip"), 4, 1), new SpriteManager(Content.Load<Texture2D>("sheep"), 1, 1)));
            players.Add(new viking(PlayerIndex.Two, new Vector2(800, 100), graphics, new SpriteManager(Content.Load<Texture2D>("vikingGreenStrip"), 4, 1), new SpriteManager(Content.Load<Texture2D>("sheep"), 1, 1)));
            players.Add(new viking(PlayerIndex.Three, new Vector2(100, 800), graphics, new SpriteManager(Content.Load<Texture2D>("vikingRedStrip"), 4, 1), new SpriteManager(Content.Load<Texture2D>("sheep"), 1, 1)));
            players.Add(new viking(PlayerIndex.Four, new Vector2(800, 800), graphics, new SpriteManager(Content.Load<Texture2D>("vikingYellowStrip"), 4, 1), new SpriteManager(Content.Load<Texture2D>("sheep"), 1, 1)));
        }

        protected override void UnloadContent()
        {
        }


        protected override void Update(GameTime gameTime)
        {
            gamePadState = GamePad.GetState(PlayerIndex.One);
            keyboardState = Keyboard.GetState();

            // Allows the default game to exit on Xbox 360 and Windows.
            if (gamePadState.Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here.
            checkPauseKey();
            if (paused == false)
            {
                base.Update(gameTime);

                Simulate(gameTime);
            }
        }

        public void Simulate(GameTime gameTime)
        {
            timeSpan -= gameTime.ElapsedGameTime;
            timeSpanLight -= gameTime.ElapsedGameTime;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (timeSpanLight.Seconds == 0)
            {
                if (health.Count < 1 && rand.Next(0, 20) == 0 && bombUp.Count < 1)
                    health.Add(new healthPowerUp(new SpriteManager(Content.Load<Texture2D>("healthpack"), 1, 1), new Vector2(512, 512)));
                else
                {
                    if (health.Count < 1 && bombUp.Count < 1 && rand.Next(0, 10) == 0)
                        bombUp.Add(new BombPowerUp(new SpriteManager(Content.Load<Texture2D>("bomb"), 1, 1), new Vector2(512, 512)));
                }

                mask.active = true;
                timeSpanLight = new TimeSpan(0, 0, rand.Next(1, 3));
            }

            mask.incBounceFrame();

            foreach (viking j in players)
            {
                j.player_update(gameTime);
                for (int p = 0; p < players.Count; p++)
                {
                    Rectangle playerRectangle = new Rectangle((int)players[p].position.X, (int)players[p].position.Y, j.playerTex.texWidth, j.playerTex.texHeight);
                    for (int i = 0; i < j.proxList.Count; i++)
                    {
                        if (playerRectangle.Intersects(j.proxList[i].sheepRectangle) && j.proxList[i].active)
                        {
                            players[p].lives--;
                            players[p].vib_duration = new TimeSpan(0, 0, 0, 0, 150);

                            j.proxList.RemoveAt(i);
                        }
                    }
                    for (int x = 0; x < health.Count; x++)
                    {
                        if (playerRectangle.Intersects(health[x].colissionRec))
                        {
                            players[p].lives++;
                            health.RemoveAt(x);
                        }
                    }
                    for (int x = 0; x < bombUp.Count; x++)
                    {
                        if (playerRectangle.Intersects(bombUp[x].colissionRec))
                        {
                            players[p].sheepBombs++;
                            bombUp.RemoveAt(x);
                        }
                    }
                }
            }
            for (int x = 0; x < players.Count; x++)
            {
                if (players[x].lives <= 0)
                {
                    deadPLayers[x] = 1;
                    players[x].visible = false;
                }
            }

            base.Update(gameTime);
        }

        void checkPauseKey()
        {
            if (gamePadState.Buttons.Start == ButtonState.Pressed)
            {
                pauseKeyDown = true;
            }
            else if (pauseKeyDown)
            {
                pauseKeyDown = false;
                paused = !paused;
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            string sheepName = "";
            switch (sheepType)
            {
                case 0: sheepName = "Proximity Sheep";
                    break;
            }
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            //background tiles
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone);
                spriteBatch.Draw(backTile, Vector2.Zero, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            spriteBatch.End();

            //player level objects
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);
                spriteBatch.Draw(statue, new Vector2(512, 512), new Rectangle(0, 0, statue.Width, statue.Height), Color.White, 0, new Vector2(statue.Width/2, statue.Height/2), 1, SpriteEffects.None, 1);
                foreach (healthPowerUp x in health)
                    x.Draw(spriteBatch);
                foreach (BombPowerUp x in bombUp)
                    x.Draw(spriteBatch);
                for (int i = 0; i < players.Count; i++)
                {
                    //if (deadPLayers[i] == 0)
                        players[i].Draw(spriteBatch);

                }
            spriteBatch.End();

            //mask over screen
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);
                mask.Draw(spriteBatch);
            spriteBatch.End();

            //HUD
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone);
                if (timeSpan > TimeSpan.FromMinutes(0))
                    spriteBatch.DrawString(HUDfont, (timeSpan.Minutes*60 + timeSpan.Seconds).ToString()+"."+timeSpan.Milliseconds.ToString()+" seconds", timeLoc, Color.Azure);
                spriteBatch.DrawString(HUDfont, "Sheep type: " + sheepName, new Vector2(380, 70), Color.Azure);
                for (int i = 0; i < playerHUD.Count; i++)
                {
                    spriteBatch.DrawString(HUDfont, "Player " + (i + 1), playerHUD[i], Color.Azure);
                    if (deadPLayers[i] > 0)
                    {
                        spriteBatch.DrawString(HUDfont, "YOU ARE DEAD", new Vector2(playerHUD[i].X, playerHUD[i].Y + 20), Color.Azure);
                    }
                    else
                    {
                        spriteBatch.DrawString(HUDfont, "Bombs: " + players[i].sheepBombs, new Vector2(playerHUD[i].X, playerHUD[i].Y + 20), Color.Azure);
                        spriteBatch.Draw(heart, new Vector2(playerHUD[i].X, playerHUD[i].Y + 40), new Rectangle((int)0, 0, (int)players[i].lives * 32, (int)32), Color.White);
                    }
                }
            spriteBatch.End();

            

            base.Draw(gameTime);
        }

        
    }
}
