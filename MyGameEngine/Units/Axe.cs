﻿using System.Collections.Generic;

namespace MyGameEngine
{
    public class Axe : Unit
    {
        public Axe() : base(Units.AXE, "Могул Хан", Types.Hero, 700, 0, 2, (52, 56), 12,
            new HashSet<Abilities> {Abilities.FireDamage})
        {
        }
    }
}