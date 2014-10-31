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

namespace PixelShooter
{
    public class Player
    {
        public String ID { get; set; }
        public Texture2D LeftTexture { get; set; }
        public Texture2D RightTexture { get; set; }
        public Boolean FacingLeft { get; set; }
        public Vector2 Position { get; set; }

        public Player(String id, Texture2D ltex, Texture2D rtex, Boolean left, Vector2 pos)
        {
            this.ID = id;
            this.LeftTexture = ltex;
            this.RightTexture = rtex;
            this.FacingLeft = left;
            this.Position = pos;
        }
        public Player(String id, Texture2D ltex, Texture2D rtex, Vector2 pos) : this(id, ltex, rtex, false, pos) { }
        public Player(String id, Texture2D ltex, Texture2D rtex) : this(id, ltex, rtex, false, new Vector2(0, 0)) { }
        public Player(String id, ContentManager content) : this(id, content.Load<Texture2D>(String.Format("{0}_left", id)),
            content.Load<Texture2D>(String.Format("{0}_right", id)), false, new Vector2(0, 0)) { }

        public Texture2D GetCurrentTexture()
        {
            return this.FacingLeft ? this.LeftTexture : this.RightTexture;
        }

        public void Move(int x, int y)
        {
            this.Position = new Vector2(x, y);
        }

        public override string ToString()
        {
            return String.Format("Player {0} at {1}, {2}", this.ID, this.Position.X, this.Position.Y);
        }
    }

    public class Button
    {
        public Texture2D texture { get; set; }
        public Vector2 position { get; set; }

        public Button(Texture2D tex, Vector2 pos)
        {
            this.texture = tex;
            this.position = pos;
        }
    }

    public class GamePixelShooter : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player p1;
        Player p2;

        public GamePixelShooter()
        {
            Window.Title = "Pixel Shooter";
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            p1 = new Player("p1", Content);
            p2 = new Player("p2", Content);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(p1.GetCurrentTexture(), p1.Position, Color.White);
            spriteBatch.Draw(p2.GetCurrentTexture(), p2.Position, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
