using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PixelShooter
{
    public class Entity
    {
        public Int32 VX { get; set; }
        public Int32 VY { get; set; }
        public Int32 AX { get; set; }
        public Int32 AY { get; set; }

        public Entity()
        {
            this.VX = 0;
            this.VY = 0;
            this.AX = 0;
            this.AY = 0;
        }
    }

    public class PhysicsEngine
    {
        public List<Entity> entities = new List<Entity>();
        public Double Gravity { get; set; }
        
        public PhysicsEngine()
        {
            this.Gravity = 9.81;
        }

        public Entity CreateEntity()
        {
            Entity e = new Entity();
            entities.Add(e);
            return e;
        }

        public void Update()
        {
        }
    }
}
