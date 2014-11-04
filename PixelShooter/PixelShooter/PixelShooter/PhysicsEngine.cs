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
    public class EntityRectangle
    {
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public Int32 Width { get; set; }
        public Int32 Height { get; set; }
        public Int32 Left
        {
            get { return this.X; }
            set { this.X = value; }
        }
        public Int32 Top
        {
            get { return this.Y; }
            set { this.Y = value; }
        }
        public Int32 Right
        {
            get { return this.X + this.Width; }
            set { this.X = value - this.Width; }
        }
        public Int32 Bottom
        {
            get { return this.Y + this.Height; }
            set { this.Y = value - this.Height; }
        }
        public Int32 CenterX
        {
            get { return this.X + this.Width/2; }
            set { this.X = value - this.Width/2; }
        }
        public Int32 CenterY
        {
            get { return this.Y + this.Height/2; }
            set { this.Y = value - this.Height/2; }
        }

        public EntityRectangle(Int32 x, Int32 y, Int32 width, Int32 height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public EntityRectangle(Rectangle rect) :
            this(rect.X, rect.Y, rect.Width, rect.Height) { }

        public void MoveTo(Int32 x, Int32 y)
        { this.X = x; this.Y = y; }
        public void MoveTo(Point pos)
        { this.MoveTo(pos.X, pos.Y); }

        public void MoveBy(Int32 x, Int32 y)
        { this.X += x; this.Y += y; }
        public void MoveBy(Point pos)
        { this.MoveBy(pos.X, pos.Y); }

        public Boolean CollidesWith(EntityRectangle other)
        {
            throw new NotImplementedException();
        }
    }

    public class Entity
    {
        public EntityRectangle Rect { get; set; }
        public Int32 VX { get; set; }
        public Int32 VY { get; set; }
        public Int32 AX { get; set; }
        public Int32 AY { get; set; }
        public Boolean Grounded { get; set; }
        public Boolean Fixed { get; set; }
        public Boolean IsAttack { get; set; }
        public String Owner { get; set; }
        public Int32 Life { get; set; }

        public Entity(Point pos, Point size, Boolean fixd, Boolean attack, String id)
        {
            this.Rect = new EntityRectangle(pos.X, pos.Y, size.X, size.Y);
            this.VX = 0;
            this.VY = 0;
            this.AX = 0;
            this.AY = 2;
            this.Fixed = fixd;
            this.Grounded = false;
            this.IsAttack = attack;
            this.Owner = id;
            this.Life = 60;
        }

        public void SetV(Int32 vx, Int32 vy)
        {
            this.VX = vx;
            this.VY = vy;
        }

        public void SetA(Int32 ax, Int32 ay)
        {
            this.AX = ax;
            this.AY = ay;
        }
    }

    public class PhysicsEngine
    {
        public List<Entity> Entities = new List<Entity>();
        public Rectangle ScreenBorders { get; set; }
        public Texture2D AttackTexture { get; set; }
        
        public PhysicsEngine()
        {
        }

        public void SetBorders(Rectangle bounds)
        {
            this.ScreenBorders = bounds;
        }

        public void AddTextures(ContentManager content)
        {
            this.AttackTexture = content.Load<Texture2D>("shuriken");
        }

        public Entity CreateEntity(Point pos, Point size, Boolean fixd, Boolean attack, String id)
        {
            Entity e = new Entity(pos, size, fixd, attack, id);
            this.Entities.Add(e);
            return e;
        }

        public void AssignEntity(Player p, Point pos, Point size)
        {
            p.Entity = this.CreateEntity(pos, size, false, false, p.ID);
        }

        public void MoveEntity(Entity e)
        {
            if (!e.Fixed)
            {
                e.VX += e.AX;
                e.VY += e.AY;
                e.Rect.MoveBy(e.VX, e.VY);

                if (e.Rect.Left < this.ScreenBorders.Left)
                    e.Rect.MoveTo(this.ScreenBorders.Right - e.Rect.Width, e.Rect.Y);

                if (e.Rect.Right > this.ScreenBorders.Right)
                    e.Rect.MoveTo(this.ScreenBorders.Left, e.Rect.Y);

                if (e.Rect.Top < this.ScreenBorders.Top)
                    e.Rect.MoveTo(e.Rect.X, this.ScreenBorders.Top);

                if (e.Rect.Bottom > this.ScreenBorders.Bottom)
                    e.Rect.MoveTo(e.Rect.X, this.ScreenBorders.Bottom - e.Rect.Height);
            }
        }

        public void UpdateGrounded(Entity e)
        {
            e.Grounded = (e.Rect.Bottom >= this.ScreenBorders.Bottom);
        }

        public void UpdateAttacks(Entity e)
        {
            if (e.Grounded)
            {
                e.SetV(0, 0);
                e.SetA(0, 0);
            }
            --e.Life;
        }

        public void Update()
        {
            for (int i = 0; i < this.Entities.Count; ++i)
                if (this.Entities.ElementAt(i).Life < 0)
                    this.Entities.RemoveAt(i);
            foreach (Entity e in this.Entities)
            {
                this.MoveEntity(e);
                this.UpdateGrounded(e);
                if (e.IsAttack)
                    this.UpdateAttacks(e);
            }
        }

        public void DrawAttacks(SpriteBatch batch)
        {
            foreach (Entity e in this.Entities)
                if (e.IsAttack)
                    batch.Draw(this.AttackTexture, new Vector2(e.Rect.Left, e.Rect.Top), Color.White);
        }
    }
}
