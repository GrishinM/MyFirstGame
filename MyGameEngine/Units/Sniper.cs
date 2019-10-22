﻿using System.Collections.Generic;

namespace MyGameEngine
{
    public class Sniper : Unit
    {
        public Sniper() : base(Units.SNIPER, "Кардел Остроглаз", Types.Hero, 580, 0, 2, (36, 42), 22,
            new HashSet<Abilities> {Abilities.Headshot})
        {
        }
    }
}