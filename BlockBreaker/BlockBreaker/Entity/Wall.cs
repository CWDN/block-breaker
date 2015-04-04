﻿using System.Collections.Generic;
using Atom;
using Atom.Entity;
using Atom.Physics;
using Atom.Physics.Collision.BoundingBox;

namespace BlockBreaker.Entity 
{
    public class Wall : BaseEntity
    {
        protected override List<Component> CreateDefaultComponents()
        {
            return new List<Component>
            {
                new PositionComponent()
                {
                    X = -100,
                    Y = 0
                },
                new BoundingBoxComponent()
                {
                    Width = 100,
                    Height = GameServices.Graphics.PreferredBackBufferHeight,
                    Active = false
                }
            };
        }
    }
}
