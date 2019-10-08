﻿using System.Collections.Generic;

namespace MyGameEngine
{
    public class Huskar : Unit
    {
        public Huskar() : base(Units.HUSKAR, "Хускар", Types.Hero, 620, 0, 1, (40, 45), 0,
            new HashSet<Abilities> {Abilities.Disarm})
        {
        }
    }
}