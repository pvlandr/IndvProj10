﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndvProj10
{
    public class Box : Unit
    {
        public override Texture2D Sprite => Sprites.Circle;

        public override PushBehavior GetPushBehavior()
        {
            return PushBehavior.Push;
        }

        public override Unit Clone()
        {
            return new Box()
            {
                Position = Position,
            };
        }
    }
}
