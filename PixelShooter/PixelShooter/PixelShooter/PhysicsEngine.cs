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
    public class Entity
    {
        public Int32 Left { get; set; }
        public Int32 Top { get; set; }
        public Int32 VX { get; set; }
        public Int32 VY { get; set; }
        public Int32 AX { get; set; }
        public Int32 AY { get; set; }

        public Entity(Vector2 pos)
        {
            this.Left += (int)pos.X;
            this.Top += (int)pos.Y;
            this.VX = 0;
            this.VY = 0;
            this.AX = 0;
            this.AY = 0;
        }

        public void Move()
        {
            this.VX += this.AX;
            this.VY += this.AY;
            this.Left += VX;
            this.Top += VY;
        }

        public void SetV()
        { }

        public void SetA()
        { }
    }

    public class PhysicsEngine
    {
        public List<Entity> Entities = new List<Entity>();
        public Double Gravity { get; set; }
        
        public PhysicsEngine()
        {
            this.Gravity = 9.81;
        }

        public Entity CreateEntity(Vector2 pos)
        {
            Entity e = new Entity(pos);
            this.Entities.Add(e);
            return e;
        }

        public void AssignEntity(Player p, Vector2 pos)
        {
            p.Entity = this.CreateEntity(pos);
        }

        public void Update()
        {
            foreach (Entity e in this.Entities)
                e.Move();
        }
    }
}
