﻿using System.Collections.Generic;

namespace MyGameEngine
{
    public class Juggernaut : Unit
    {
        public Juggernaut() : base(Units.JUGGERNAUT, "Юрнеро", Types.Hero, 600, 0, 5, (46, 50), 0,
            new HashSet<Abilities> {Abilities.ImmuneToFire})
        {
        }
    }
}