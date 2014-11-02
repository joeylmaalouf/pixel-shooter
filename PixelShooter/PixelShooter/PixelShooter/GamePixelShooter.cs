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
        public ControlScheme Controls { get; set; }
        public Texture2D LeftTexture { get; set; }
        public Texture2D RightTexture { get; set; }
        public Boolean FacingLeft { get; set; }
        public Vector2 Position { get; set; }
        public Int32 Shots { get; set; }

        public Player(String id, ControlScheme controls, Texture2D ltex, Texture2D rtex, Boolean left, Vector2 pos)
        {
            this.ID = id;
            this.Controls = controls;
            this.LeftTexture = ltex;
            this.RightTexture = rtex;
            this.FacingLeft = left;
            this.Position = pos;
            this.Shots = 3;
        }
        
        public Player(String id, ControlScheme controls, Texture2D ltex, Texture2D rtex, Vector2 pos) :
            this(id, controls, ltex, rtex, false, pos) { }

        public Player(String id, ControlScheme controls, Texture2D ltex, Texture2D rtex) :
            this(id, controls, ltex, rtex, false, new Vector2(0, 0)) { }
        
        public Player(String id, ContentManager content, ControlScheme controls) :
            this(id, controls, content.Load<Texture2D>(String.Format("{0}_left", id)), content.Load<Texture2D>(String.Format("{0}_right", id))) { }

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

    public class ControlScheme
    {
        public Keys Left { get; set; }
        public Keys Right { get; set; }
        public Keys Jump { get; set; }
        public Keys Catch { get; set; }
        public Keys Shoot { get; set; }

        public ControlScheme(Keys l, Keys r, Keys j, Keys c, Keys s)
        {
            this.Left = l;
            this.Right = r;
            this.Jump = j;
            this.Catch = c;
            this.Shoot = s;
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
        PhysicsEngine engine = new PhysicsEngine();
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
            p1 = new Player("p1", Content, new ControlScheme(Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.RightAlt));
            p2 = new Player("p2", Content, new ControlScheme(Keys.A, Keys.D, Keys.W, Keys.S, Keys.Space));
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            engine.Update();
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
