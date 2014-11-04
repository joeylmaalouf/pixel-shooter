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
        public Int32 Cooldown { get; set; }
        public Int32 AnimationOffset { get; set; }

        public Player(String id, ControlScheme controls, Texture2D sheet)
        {
            this.Entity = null;
            this.ID = id;
            this.Controls = controls;
            this.SpriteSheet = sheet;
            this.Cooldown = 0;
            this.AnimationOffset = 0;
        }

        public Player(String id, ControlScheme controls, ContentManager content) :
            this(id, controls, content.Load<Texture2D>(id)) { }

        public void DrawSpriteInBatch(SpriteBatch batch, Int32 frame)
        {
            Int32 slot = (frame%30)/15;
            Rectangle source = new Rectangle(slot * 64 + this.AnimationOffset, 0, 64, 64);
            batch.Draw(this.SpriteSheet, new Vector2(this.Entity.Rect.Left, this.Entity.Rect.Top), source, Color.White);
        }

        public void UpdateEntityFromInput(PhysicsEngine engine)
        {
            if (Keyboard.GetState().IsKeyDown(this.Controls.Left))
            {
                this.Entity.VX = -10;
                this.AnimationOffset = 0;
            }
            if (Keyboard.GetState().IsKeyDown(this.Controls.Right))
            {
                this.Entity.VX = 10;
                this.AnimationOffset = 256;
            }
            if (!(Keyboard.GetState().IsKeyDown(this.Controls.Left) ^ Keyboard.GetState().IsKeyDown(this.Controls.Right)))
                this.Entity.VX = 0;
            if (this.Entity.Grounded && Keyboard.GetState().IsKeyDown(this.Controls.Jump))
            {
                this.Entity.VY = -30;
                this.AnimationOffset = 128;
            }
            if (this.Cooldown <= 0 && Keyboard.GetState().IsKeyDown(this.Controls.Shoot))
            {
                Point pos = new Point(this.Entity.Rect.CenterX, this.Entity.Rect.CenterY);
                Point size = new Point(16, 16);
                Entity fireball = engine.CreateEntity(pos, size, false, true, this.ID);
                if (this.AnimationOffset == 128)
                    fireball.SetV(0, -30);
                else if (this.AnimationOffset == 0)
                    fireball.SetV(-40, -10);
                else if (this.AnimationOffset == 256)
                    fireball.SetV(40, -10);
                this.Cooldown = 45;
            }
            --this.Cooldown;
        }

        public void CheckAlive(PhysicsEngine engine)
        {
            foreach (Entity e in engine.Entities)
                if (e.IsAttack && this.Entity.Rect.CollidesWith(e.Rect))
                {
                }
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
            graphics.PreferredBackBufferWidth = 1080;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsFixedTimeStep = true;
            IsMouseVisible = true;
            frame = 0;
            engine.SetBorders(GraphicsDevice.Viewport.Bounds);
            engine.AddTextures(Content);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            p1 = new Player("orange", new ControlScheme(Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.RightAlt), Content);
            p2 = new Player("blue", new ControlScheme(Keys.A, Keys.D, Keys.W, Keys.S, Keys.Space), Content);
            engine.AssignEntity(p1, new Point(100, 100), new Point(64, 64));
            engine.AssignEntity(p2, new Point(200, 100), new Point(64, 64));
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            p1.UpdateEntityFromInput(engine);
            p2.UpdateEntityFromInput(engine);
            p1.CheckAlive(engine);
            p2.CheckAlive(engine);
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
            engine.DrawAttacks(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
