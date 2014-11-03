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
        public Texture2D SpriteSheet { get; set; }
        public Int32 Shots { get; set; }

        public Player(String id, ControlScheme controls, Texture2D sheet)
        {
            this.Entity = null;
            this.ID = id;
            this.Controls = controls;
            this.SpriteSheet = sheet;
            this.Shots = 3;
        }

        public Player(String id, ControlScheme controls, ContentManager content) :
            this(id, controls, content.Load<Texture2D>(id)) { }

        public void DrawSpriteInBatch(SpriteBatch batch, Int32 frame)
        {
            Int32 slot = (frame%30)/15;
            Rectangle source = new Rectangle(slot*32, 0, 32, 32);
            batch.Draw(this.SpriteSheet, new Vector2(this.Entity.Rect.Left, this.Entity.Rect.Top), source, Color.White);
        }

        public override string ToString()
        {
            return String.Format("player \"{0}\" at position {1}, {2}", this.ID, this.Entity.Rect.X, this.Entity.Rect.Y);
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

    public class GamePixelShooter : Microsoft.Xna.Framework.Game
    {
        Int32 frame;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PhysicsEngine engine = new PhysicsEngine();
        Player p1;
        Player p2;

        public GamePixelShooter()
        {
            Window.Title = "Pixel Shooter";
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsFixedTimeStep = true;
            IsMouseVisible = true;
            frame = 0;
            engine.SetBorders(GraphicsDevice.Viewport.Bounds);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            p1 = new Player("orange", new ControlScheme(Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.RightAlt), Content);
            p2 = new Player("blue", new ControlScheme(Keys.A, Keys.D, Keys.W, Keys.S, Keys.Space), Content);
            engine.AssignEntity(p1, new Point(100, 100), new Point(32, 32));
            engine.AssignEntity(p2, new Point(200, 100), new Point(32, 32));
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            engine.Update();
            ++frame;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            p1.DrawSpriteInBatch(spriteBatch, frame);
            p2.DrawSpriteInBatch(spriteBatch, frame);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
