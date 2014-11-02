using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public Entity Entity { get; set; }
        public String ID { get; set; }
        public ControlScheme Controls { get; set; }
        public Texture2D LeftTexture { get; set; }
        public Texture2D RightTexture { get; set; }
        public Boolean FacingLeft { get; set; }
        public Int32 Shots { get; set; }

        public Player(String id, ControlScheme controls, Texture2D ltex, Texture2D rtex, Boolean left)
        {
            this.Entity = null;
            this.ID = id;
            this.Controls = controls;
            this.LeftTexture = ltex;
            this.RightTexture = rtex;
            this.FacingLeft = left;
            this.Shots = 3;
        }

        public Player(String id, ControlScheme controls, Texture2D ltex, Texture2D rtex) :
            this(id, controls, ltex, rtex, false) { }

        public Player(String id, ControlScheme controls, ContentManager content) :
            this(id, controls, content.Load<Texture2D>(String.Format("{0}_left", id)), content.Load<Texture2D>(String.Format("{0}_right", id))) { }

        public Texture2D GetCurrentTexture()
        {
            return this.FacingLeft ? this.LeftTexture : this.RightTexture;
        }

        public override string ToString()
        {
            return String.Format("player \"{0}\" at position {1}, {2}", this.ID, this.Entity.Left, this.Entity.Top);
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
            p1 = new Player("p1", new ControlScheme(Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.RightAlt), Content);
            p2 = new Player("p2", new ControlScheme(Keys.A, Keys.D, Keys.W, Keys.S, Keys.Space), Content);
            engine.AssignEntity(p1, new Vector2(100, 100));
            engine.AssignEntity(p2, new Vector2(200, 100));
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
            spriteBatch.Draw(p1.GetCurrentTexture(), new Vector2(p1.Entity.Left, p1.Entity.Top), Color.White);
            spriteBatch.Draw(p2.GetCurrentTexture(), new Vector2(p2.Entity.Left, p2.Entity.Top), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
