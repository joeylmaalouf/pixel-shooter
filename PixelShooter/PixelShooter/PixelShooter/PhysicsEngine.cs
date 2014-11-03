﻿using System;
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
        public Boolean Fixed { get; set; }

        public Entity(Vector2 pos, Boolean fixd)
        {
            this.Left += (int)pos.X;
            this.Top += (int)pos.Y;
            this.VX = 0;
            this.VY = 0;
            this.AX = 0;
            this.AY = 0;
            this.Fixed = fixd;
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
        public Double Gravity { get; set; }
        
        public PhysicsEngine()
        {
            this.Gravity = 9.81;
        }

        public Entity CreateEntity(Vector2 pos, Boolean fixd)
        {
            Entity e = new Entity(pos, fixd);
            this.Entities.Add(e);
            return e;
        }

        public void AssignEntity(Player p, Vector2 pos)
        {
            p.Entity = this.CreateEntity(pos, false);
        }

        public void MoveEntity(Entity e)
        {
            if (!e.Fixed)
            {
                e.VX += e.AX;
                e.VY += e.AY;
                e.Left += e.VX;
                e.Top += e.VY;
            }
        }

        public void Update()
        {
            foreach (Entity e in this.Entities)
                this.MoveEntity(e);
        }
    }
}
