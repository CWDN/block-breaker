﻿using System.Collections.Generic;
using Atom;
using Atom.Entity;
using Atom.Graphics.Rendering;
using Atom.Physics;
using Atom.Physics.Collision.BoundingBox;
using BlockBreaker.Entity.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlockBreaker.Entity
{
    public class Block : BaseEntity
    {
        protected override List<Component> CreateDefaultComponents()
        {
            float middleOfScreen = (float)GameServices.Graphics.PreferredBackBufferWidth / 2;
            int blockHeight = 73;
            int blockWidth = 73;

            float scale = 1F;

            List<Component> components = new List<Component>
            {
                new PositionComponent() {X = middleOfScreen - blockWidth * scale / 2, Y = (blockHeight * scale) + 70 },
                new VelocityComponent(),
                new BoundingBoxComponent()
                {
                    Active = false,
                    Width = (int) (blockWidth * scale),
                    Height = (int) (blockHeight * scale),
                },
                new SpriteComponent()
                {
                    Image = Content.Load<Texture2D>("blockSpritesheet"),
                    Location = new Point(0, 0),
                    FrameHeight = blockHeight,
                    FrameWidth = blockWidth,
                    Scale = scale
                },
                new HealthComponent()
                {
                    MaxHealth = 7,
                    Health = 7
                }
            };

            return components;
        }
    }
}