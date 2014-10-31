using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PixelShooter
{
    public class Entity
    {
        public Entity()
        {
        }
    }

    public class JPhysicsEngine
    {
        public List<Entity> entities = new List<Entity>();
        public Double Gravity { get; set; }
        
        public JPhysicsEngine()
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
