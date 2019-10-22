﻿using System.Collections.Generic;

namespace MyGameEngine
{
    public class Lich : Unit
    {
        public Lich() : base(Units.LICH, "Этриан", Types.Hero, 600, 0, 1, (50, 59), 18,
            new HashSet<Abilities> {Abilities.Dark, Abilities.Shield})
        {
        }
    }
}