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
        public Boolean Alive { set; get; }

        public Player(String id, ControlScheme controls, Texture2D sheet)
        {
            this.Entity = null;
            this.ID = id;
            this.Controls = controls;
            this.SpriteSheet = sheet;
            this.Cooldown = 0;
            this.AnimationOffset = 0;
            this.Alive = true;
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
            if (this.Alive)
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
                    Point pos = new Point(this.Entity.Rect.CenterX - 8, this.Entity.Rect.CenterY - 8);
                    Point size = new Point(16, 16);
                    Entity projectile = engine.CreateEntity(pos, size, false, true, this.ID);
                    if (this.AnimationOffset == 128)
                        projectile.SetV(0, -20);
                    else if (this.AnimationOffset == 0)
                        projectile.SetV(-30, -10);
                    else if (this.AnimationOffset == 256)
                        projectile.SetV(30, -10);
                    this.Cooldown = 45;
                }
            }
            else
            {
                this.Entity.SetV(0, 0);
                this.Entity.SetA(0, 0);
            }
            --this.Cooldown;
        }

        public void CheckAlive(PhysicsEngine engine)
        {
            foreach (Entity e in engine.Entities)
                if (e.IsAttack && !e.Grounded && e.Owner != this.ID && e.Rect.CollidesWith(this.Entity.Rect))
                {
                    this.AnimationOffset = 384;
                    this.Alive = false;
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
        public Keys Reset { get; set; }

        public ControlScheme(Keys l, Keys r, Keys j, Keys c, Keys s, Keys e)
        {
            this.Left = l;
            this.Right = r;
            this.Jump = j;
            this.Catch = c;
            this.Shoot = s;
            this.Reset = e;
        }
    }

    public class GamePixelShooter : Microsoft.Xna.Framework.Game
    {
        Int32 frame;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont myFont;
        PhysicsEngine engine = new PhysicsEngine();
        Player p1, p2;
        String title, go;
        Vector2 TitleOrigin, TitlePosition, GameOverOrigin, GameOverPosition;

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
            this.ResetGame();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            myFont = Content.Load<SpriteFont>("LucidaConsole");
            title = "Pixel Shooter";
            go = String.Format("Game Over. Press \"{0}\" or \"{1}\" to play again.", p1.Controls.Reset, p2.Controls.Reset);
            TitleOrigin = myFont.MeasureString(title) / 2;
            TitlePosition = new Vector2(graphics.PreferredBackBufferWidth / 2 - TitleOrigin.X, TitleOrigin.Y);
            GameOverOrigin = myFont.MeasureString(go) / 2;
            GameOverPosition = new Vector2(graphics.PreferredBackBufferWidth / 2 - GameOverOrigin.X, graphics.PreferredBackBufferHeight / 2 - GameOverOrigin.Y);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(p1.Controls.Reset) || Keyboard.GetState().IsKeyDown(p2.Controls.Reset))
                this.ResetGame();
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
            spriteBatch.DrawString(myFont, title, TitlePosition, Color.White);
            if (!p1.Alive || !p2.Alive)
                spriteBatch.DrawString(myFont, go, GameOverPosition, Color.Red);
            p1.DrawSpriteInBatch(spriteBatch, frame);
            p2.DrawSpriteInBatch(spriteBatch, frame);
            engine.DrawAttacks(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void ResetGame()
        {
            engine.Entities.Clear();
            p1 = new Player("orange", new ControlScheme(Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.RightAlt, Keys.L), Content);
            p2 = new Player("blue", new ControlScheme(Keys.A, Keys.D, Keys.W, Keys.S, Keys.Space, Keys.R), Content);
            engine.AssignEntity(p1, new Point(200, 100), new Point(64, 64));
            engine.AssignEntity(p2, new Point(graphics.PreferredBackBufferWidth - 200 - 64, 100), new Point(64, 64));
        }
    }
}
