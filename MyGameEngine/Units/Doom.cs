﻿using System.Collections.Generic;

namespace MyGameEngine
{
    public class Doom : Unit
    {
        public Doom() : base(Units.DOOM, "Люцифер", Types.Hero, 720, 0, 1, (53, 69), 13,
            new HashSet<Abilities> {Abilities.Dark, Abilities.FireDamage, Abilities.Break})
        {
        }
    }
}